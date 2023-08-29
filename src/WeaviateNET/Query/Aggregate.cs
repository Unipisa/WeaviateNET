using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET.Query.AdditionalOperator;
using WeaviateNET.Query.AggregateOperator;

namespace WeaviateNET.Query
{
    public class Aggregate<P> : QueryBlock where P : class, new()
    {
        private WeaviateClass<P> _class;
        private Filter<P> _filter;
        private List<AggregateOperator.AggregateOperator<P>> _operators;

        private bool groupedBy = false;

        public Filter<P> Filter { get { return _filter; } }
        public bool MetaCount { get; set; } = true;

        internal Aggregate(WeaviateClass<P> @class)
        {
            _class = @class;
            Output = new StringBuilder();
            _filter = new Filter<P>(QueryType.Aggregate);
            _operators = new List<AggregateOperator.AggregateOperator<P>>();
        }

        public Aggregate<P> Meta(bool include = true)
        {
            MetaCount = include;
            return this;
        }
        public Aggregate<P> TextStats(string propertyName, bool count = true, bool type = true, bool topOccurrences = false)
        {
            _operators.Add(new TextStats<P>(propertyName) { Count = count, Type = type, TopOccurrences = topOccurrences });
            return this;
        }
        public Aggregate<P> NumberStats(string propertyName, bool count = true,  bool type = true, bool minimum = false, 
            bool maximum = false, bool mean = false, bool median = false, bool mode = false, bool sum = false)
        {
            _operators.Add(new NumberStats<P>(propertyName) { 
                Count = count,
                Type = type, 
                Minimum = minimum,
                Maximum = maximum,
                Mean = mean,
                Median = median, 
                Mode = mode, 
                Sum = sum });
            return this;
        }
        public Aggregate<P> BoolStats(string propertyName, bool count = true, bool type = true, bool totalTrue = false,
            bool totalFalse = false, bool percentageTrue = false, bool percentageFalse = false)
        {
            _operators.Add(new BoolStats<P>(propertyName) { 
                Count = count,
                Type = type,
                TotalTrue = totalTrue, 
                TotalFalse = totalFalse,
                PercentageTrue = percentageTrue,
                PercentageFalse = percentageFalse
            });
            return this;
        }
        public Aggregate<P> GroupedBy(bool include=true)
        {
            groupedBy = true;
            return this;
        }

        protected override void Render()
        {
            groupedBy = (_filter.Operators!.Where(o => o is GroupBy<P>).Any());

            AppendLineStartBlock("{");
            AppendLineStartBlock("Aggregate {");
            if (!_filter.IsEmpty)
            {
                AppendLineStartBlock($"{_class.Name} (");
                _filter.Render(Output!, IndentationLevel);
                AppendLineBetweenBlocks(") {");
                if (groupedBy)
                {
                    AppendLineStartBlock("groupedBy {");
                    AppendLine("path");
                    AppendLine("value");
                    AppendLineEndBlock("}");
                }

                if (MetaCount)
                {
                    AppendLineStartBlock("meta {");
                    AppendLine("count");
                    AppendLineEndBlock("}");
                }
                foreach (var a in _operators)
                {
                    a.Render(Output!, IndentationLevel);
                }
                AppendLineEndBlock("}");
            }
            else
            {
                AppendLineStartBlock($"{_class.Name} {{");
                if (groupedBy)
                {
                    AppendLineStartBlock("groupedBy {");
                    AppendLine("path");
                    AppendLine("value");
                    AppendLineEndBlock("}");
                }

                if (MetaCount)
                {
                    AppendLineStartBlock("meta {");
                    AppendLine("count");
                    AppendLineEndBlock("}");
                }
                foreach (var a in _operators)
                {
                    a.Render(Output!, IndentationLevel);
                }
                AppendLineEndBlock("}");
            }

            AppendLineEndBlock("}");
            AppendLineEndBlock("}");
        }
    }
}
