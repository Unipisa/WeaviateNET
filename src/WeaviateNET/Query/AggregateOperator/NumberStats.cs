using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AggregateOperator
{
    public class NumberStats<P> : AggregateOperator<P> where P : class, new()
    {
        private string _name;
        
        public string PropertyName { get { return _name; } }

        public bool Count { get; set; } = true;
        public bool Type { get; set; } = true;
        public bool Minimum { get; set; } = false;
        public bool Maximum { get; set; } = false;
        public bool Mean { get; set; } = false;
        public bool Median { get; set; } = false;
        public bool Mode { get; set; } = false;
        public bool Sum { get; set; } = false;

        public NumberStats(string name)
        {
            var f = Schema.PersistentFields<P>().FirstOrDefault(f => f.Name == name);
            if (f == null)
            {
                throw new ArgumentException($"Property {name} does not exist on class {typeof(P).Name}");
            }
            if (f.FieldType != typeof(int) && f.FieldType != typeof(long) && f.FieldType != typeof(double))
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
            if (Minimum)
            {
                AppendLine("minimum");
            }
            if (Maximum)
            {
                AppendLine("maximum");
            }
            if (Mean)
            {
                AppendLine("mean");
            }
            if (Median)
            {
                AppendLine("median");
            }
            if (Mode)
            {
                AppendLine("mode");
            }
            if (Sum)
            {
                AppendLine("sum");
            }

            AppendLineEndBlock("}");
        }
    }
}
