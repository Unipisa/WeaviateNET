using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query
{
    public class AdditionalFields : QueryBlock
    {
        private  List<AdditionalField> _additionalFields;

        public List<AdditionalField> Fields { get { return _additionalFields; } }

        internal AdditionalFields() 
        {
            _additionalFields = new List<AdditionalField>();
        }

        public void Add(params AdditionalField[] fields)
        {
            foreach (var field in fields)
            {
                _additionalFields.Add(field);
            }
        }

        protected override void Render()
        {
            if (_additionalFields.Count == 0)
                return;

            AppendLineStartBlock("_additional {");
            foreach (var field in _additionalFields)
            {
                field.Render(Output!, IndentationLevel);
            }
            AppendLineEndBlock("}");
        }
    }
}
