using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AggregateOperator
{
    public class TextStats<P> : AggregateOperator<P> where P : class, new()
    {
        private string _name;
        
        public string PropertyName { get { return _name; } }

        public bool Count { get; set; } = true;
        public bool Type { get; set; } = true;
        public bool TopOccurrences { get; set; } = false;

        public TextStats(string name)
        {
            var f = Schema.PersistentFields<P>().FirstOrDefault(f => f.Name == name);
            if (f == null)
            {
                throw new ArgumentException($"Property {name} does not exist on class {typeof(P).Name}");
            }
            if (f.FieldType != typeof(string))
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
            if (TopOccurrences)
            {
                AppendLineStartBlock("topOccurrences {");
                AppendLine("value");
                AppendLine("occurs");
                AppendLineEndBlock("}");
            }
            AppendLineEndBlock("}");
        }
    }
}
