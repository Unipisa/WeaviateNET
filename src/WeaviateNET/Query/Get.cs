using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query
{
    public class Get<P> : QueryBlock where P : class, new()
    {
        private WeaviateClass<P> _class;
        private FieldList<P> _fields;
        private Filter<P> _filter;

        public FieldList<P> Fields { get { return _fields; } }

        public Filter<P> Filter { get { return _filter; } }

        internal Get(WeaviateClass<P> @class, bool selectall=true)
        {
            _class = @class;
            Output = new StringBuilder();
            _fields = new FieldList<P>(selectall);
            _filter = new Filter<P>(QueryType.Get);
        }

        protected override void Render()
        {
            AppendLineStartBlock("{");
            AppendLineStartBlock("Get {");
            if (!_filter.IsEmpty)
            {
                AppendLineStartBlock($"{_class.Name} (");
                _filter.Render(Output!, IndentationLevel);
                AppendLineBetweenBlocks(") {");
                _fields.Render(Output!, IndentationLevel);
                AppendLineEndBlock("}");
            }
            else
            {
                AppendLineStartBlock($"{_class.Name} {{");
                _fields.Render(Output!, IndentationLevel);
                AppendLineEndBlock("}");
            }

            AppendLineEndBlock("}");
            AppendLineEndBlock("}");
        }
    }
}
