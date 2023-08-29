using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AggregateOperator
{
    public class BoolStats<P> : AggregateOperator<P> where P : class, new()
    {
        private string _name;
        
        public string PropertyName { get { return _name; } }

        public bool Count { get; set; } = true;
        public bool Type { get; set; } = true;
        public bool TotalTrue { get; set; } = false;
        public bool TotalFalse { get; set; } = false;
        public bool PercentageTrue { get; set; } = false;
        public bool PercentageFalse { get; set; } = false;

        public BoolStats(string name)
        {
            var f = Schema.PersistentFields<P>().FirstOrDefault(f => f.Name == name);
            if (f == null)
            {
                throw new ArgumentException($"Property {name} does not exist on class {typeof(P).Name}");
            }
            if (f.FieldType != typeof(bool))
            {
                throw new ArgumentException($"Property {name} is not of type text");
            }
            _name = name;
        }

        protected override void Render()
        {
            AppendLineStartBlock($"{_name} {{");
            if (Count)
            {
                AppendLine("count");
            }
            if (Type)
            {
                AppendLine("type");
            }
            if (TotalTrue)
            {
                AppendLine("totalTrue");
            }
            if (TotalFalse)
            {
                AppendLine("totalFalse");
            }
            if (PercentageTrue)
            {
                AppendLine("percentageTrue");
            }
            if (PercentageFalse)
            {
                AppendLine("percentageFalse");
            }
            AppendLineEndBlock("}");
        }
    }
}
