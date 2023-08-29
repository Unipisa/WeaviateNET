using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.SearchOperator
{
    [GetOperator, AggregateOperator, ExploreOperator]
    public class NearText<P> : FilterOperator<P> where P : class, new()
    {
        internal bool IncludeBlock { get; set; } = true;

        public string[]? Concepts { get; set; }
        public float? Distance { get; set; }
        public float? Certainty { get; set; }
        public bool? Autocorrect { get; set; }
        public NearText<P>? MoveTo { get; set; }
        public string[]? MoveToConcepts { get; set; }
        public Guid[]? MoveToObjects { get; set; }
        public float? MoveToForce { get; set; }
        public NearText<P>? MoveAwayFrom { get; set; }
        public string[]? MoveAwayFromConcepts { get; set; }
        public Guid[]? MoveAwayFromObjects { get; set; }
        public float? MoveAwayFromForce { get; set; }

        internal NearText()
        {
        }

        protected override void Render()
        {
            if (Concepts == null)
                throw new Exception($"Parameter 'concepts' is mandatory in nearText operator");

            if (IncludeBlock)
                AppendLineStartBlock("nearText: {");
            AppendLine($"concepts: {JsonConvert.SerializeObject(Concepts)}");
            if (Distance != null)
                AppendLine($"distance: {JsonConvert.SerializeObject(Distance)}");
            if (Certainty != null)
                AppendLine($"certainty: {JsonConvert.SerializeObject(Certainty)}");
            if (Autocorrect != null)
                AppendLine($"autocorrect: {JsonConvert.SerializeObject(Autocorrect)}");
            if (MoveTo != null)
            {
                AppendLineStartBlock("moveTo: {");
                MoveTo.IncludeBlock = false;
                MoveTo.Render(Output!, IndentationLevel);
                AppendLineEndBlock("}");
            }
            if (MoveToConcepts != null)
            {
                AppendLineStartBlock("moveTo: {");
                AppendLine($"concepts: {JsonConvert.SerializeObject(MoveToConcepts)}");
                AppendLineEndBlock("}");
            }
            if (MoveToObjects != null)
            {
                AppendLineStartBlock("moveTo: {");
                AppendLine($"objects: {JsonConvert.SerializeObject(MoveToObjects)}");
                AppendLineEndBlock("}");
            }
            if (MoveToForce != null)
            {
                AppendLineStartBlock("moveTo: {");
                AppendLine($"force: {JsonConvert.SerializeObject(MoveToForce)}");
                AppendLineEndBlock("}");
            }
            if (MoveAwayFrom != null)
            {
                AppendLineStartBlock("moveAwayFrom: {");
                MoveAwayFrom.IncludeBlock = false;
                MoveAwayFrom.Render(Output!, IndentationLevel);
                AppendLineEndBlock("}");
            }
            if (MoveAwayFromConcepts != null)
            {
                AppendLineStartBlock("moveAwayFrom: {");
                AppendLine($"concepts: {JsonConvert.SerializeObject(MoveAwayFromConcepts)}");
                AppendLineEndBlock("}");
            }
            if (MoveAwayFromObjects != null)
            {
                AppendLineStartBlock("moveAwayFrom: {");
                AppendLine($"objects: {JsonConvert.SerializeObject(MoveAwayFromObjects)}");
                AppendLineEndBlock("}");
            }
            if (MoveAwayFromForce != null)
            {
                AppendLineStartBlock("moveAwayFrom: {");
                AppendLine($"force: {JsonConvert.SerializeObject(MoveAwayFromForce)}");
                AppendLineEndBlock("}");
            }
            if (IncludeBlock)
                AppendLineEndBlock("}");
        }
    }
}
