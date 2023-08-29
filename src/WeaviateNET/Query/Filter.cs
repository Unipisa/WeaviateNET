using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET.Query.AdditionalOperator;
using WeaviateNET.Query.SearchOperator;

namespace WeaviateNET.Query
{
    public enum QueryType
    {
        Get,
        Aggregate,
        Explore
    }

    public class Filter<P> : QueryBlock where P : class, new()
    {
        private QueryType _type;
        private Type _attributeQueryType;
        private List<FilterOperator<P>> _operators;

        public bool IsEmpty { get { return _operators.Count == 0; } }
        public ICollection<FilterOperator<P>>? Operators { get { return _operators; } }

        private bool CompatibleOperand(FilterOperator<P> op)
        {

            if (_type == QueryType.Aggregate)
            {
                var optype = op.GetType();
                switch (optype)
                {
                    case Type when optype == typeof(Clause<P, string>):
                        var name = ((Clause<P, string>)op).Name;
                        return (new[] {"tenant", "objectLimit"}).Contains(name);
                    case Type when optype == typeof(Clause<P, int>):
                    case Type when optype == typeof(Clause<P, Guid>):
                    case Type when optype == typeof(Clause<P, ConsistencyLevel>):
                        return false;
                }
            } 
            else if (_type == QueryType.Get)
            {
                var optype = op.GetType();
                switch (optype)
                {
                    case Type when optype == typeof(Clause<P, int>):
                        var name = ((Clause<P, int>)op).Name;
                        return !(new[] { "objectLimit" }).Contains(name);
                    case Type when optype == typeof(Clause<P, string>):
                    case Type when optype == typeof(Clause<P, Guid>):
                    case Type when optype == typeof(Clause<P, ConsistencyLevel>):
                        return true;
                }
            }
            return op.GetType().GetCustomAttribute(_attributeQueryType) != null;
        }

        public void Clear()
        {
            _operators.Clear();
        }

        public void AddOperator(FilterOperator<P> op)
        {
            if (CompatibleOperand(op))
            {
                _operators.Add(op);
            } else
            {
                throw new Exception($"Operator '{op.GetType().Name}' is not allowed in a '{_type}' query");
            }
        }

        internal Filter(QueryType type)
        {
            _type = type;
            switch (type)
            {
                case QueryType.Get:
                    _attributeQueryType = typeof(GetOperatorAttribute);
                    break;
                case QueryType.Aggregate:
                    _attributeQueryType = typeof(AggregateOperatorAttribute);
                    break;
                case QueryType.Explore:
                    _attributeQueryType = typeof(ExploreOperatorAttribute);
                    break;
                default:
                    throw new Exception("Unknown query type");
            }
            _operators = new List<FilterOperator<P>>();
        }

        public Filter<P> Where(ConditionalAtom<P> cond) { 
            AddOperator(new ConditionalOperator.Where<P>() { Operator = cond }); 
            return this; 
        }
        public Filter<P> NearVector(float[] vector, float? distance = null, float? certainty = null) {
            AddOperator(new SearchOperator.NearVector<P>() { Vector = vector, Distance = distance, Certainty = certainty });
            return this; 
        }
        public Filter<P> NearObject(Guid id, string beacon, float? distance = null, float? certainty = null)
        {
            AddOperator(new SearchOperator.NearObject<P>() { Id = id, Beacon = beacon, Distance = distance, Certainty = certainty });
            return this; 
        }
        public Filter<P> NearText(
            string concept, float? distance = null, float? certainty = null, bool? autocorrect = null,
            NearText<P>? moveTo = null, string[]? moveToConcepts = null, Guid[]? moveToObjects = null,
            float? moveToForce = null, NearText<P>? moveAwayFrom = null, string[]? moveAwayFromConcepts = null,
            Guid[]? moveAwayFromObjects = null, float? moveAwayFromForce = null)
        {
            return NearText(
                new[] { concept }, distance, certainty, autocorrect, moveTo, moveToConcepts, 
                moveToObjects, moveToForce, moveAwayFrom, moveAwayFromConcepts, moveAwayFromObjects, 
                moveAwayFromForce);
        } 
        public Filter<P> NearText(
            string[] concepts, float? distance = null, float? certainty = null, bool? autocorrect=null,
            NearText<P>? moveTo = null, string[]? moveToConcepts = null, Guid[]? moveToObjects=null,
            float? moveToForce = null, NearText<P>? moveAwayFrom = null, string[]? moveAwayFromConcepts = null,
            Guid[]? moveAwayFromObjects = null, float? moveAwayFromForce = null)
        {
            AddOperator(new SearchOperator.NearText<P>() { 
                Concepts = concepts, 
                Distance = distance, 
                Certainty = certainty,
                Autocorrect = autocorrect,
                MoveTo = moveTo,
                MoveToConcepts = moveToConcepts,
                MoveToObjects = moveToObjects,
                MoveToForce = moveToForce,
                MoveAwayFrom = moveAwayFrom,
                MoveAwayFromConcepts = moveAwayFromConcepts,
                MoveAwayFromObjects = moveAwayFromObjects,
                MoveAwayFromForce = moveAwayFromForce
            });
            return this; 
        }
        public Filter<P> Hybrid(string query, float? alpha = null, float[]? vector = null, string[]? properties = null, string? fustionType = null)
        {
            AddOperator(new Hybrid<P>() { Query = query, Alpha = alpha, Vector = vector, Properties = properties, FusionType = fustionType });
            return this;
        }
        public Filter<P> Group(GroupType type, float? force=null)
        {
            AddOperator(new Group<P>() { Type = type, Force = force });
            return this;
        }
        public Filter<P> BM25(string query, string[]? properties = null)
        {
            AddOperator(new BM25<P>() { Query = query, Properties = properties });
            return this;
        }
        public Filter<P> Ask(string question, float? certainty = null, string[]? properties = null, bool? rerank = null)
        {
            AddOperator(new Ask<P>() { Question = question, Certainty = certainty, Properties = properties, Rerank = rerank });
            return this;
        }
        public Filter<P> Limit(int v) { AddOperator(Clause<P, int>.Limit(v)); return this; }
        public Filter<P> Offset(int v) { AddOperator(Clause<P, int>.Offset(v)); return this; }
        public Filter<P> Autocut(int v) { AddOperator(Clause<P, int>.Autocut(v)); return this; }
        public Filter<P> After(Guid v) { AddOperator(Clause<P, Guid>.After(v)); return this; }
        public Filter<P> Tenant(string v) { AddOperator(Clause<P, string>.Tenant(v)); return this; }
        public Filter<P> GroupBy(string property, int? groups = null, int? objectsPerGroup = null)
        {
            return GroupBy(new[] { property }, groups, objectsPerGroup);
        }
        public Filter<P> GroupBy(string[] path, int? groups = null, int? objectsPerGroup = null) { 
            if (_type == QueryType.Get)
            {
                AddOperator(new GroupBy<P>() { Path = path, Groups = groups, ObjectsPerGroup = objectsPerGroup });
            } else if (_type == QueryType.Aggregate)
            {
                if (groups != null || objectsPerGroup != null)
                    throw new Exception("'groups' and 'objectPerGroup' should not be specified for Aggregate queries.");
                AddOperator(new GroupBy<P>() { Path = path });
            }
            else
            {
                throw new Exception($"Operator 'GroupBy' is not allowed in a '{_type}' query");
            }
            return this;
        }
        public Filter<P> ObjectLimit(int v) { AddOperator(Clause<P, int>.ObjectLimit(v)); return this; }


        protected override void Render()
        {
            if (Operators != null)
            {
                foreach (var op in _operators)
                {
                    op.Render(Output!, IndentationLevel);
                }
            }
        }
    }
}
