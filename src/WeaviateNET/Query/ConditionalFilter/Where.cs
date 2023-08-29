using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.ConditionalOperator
{
    [GetOperator, AggregateOperator, ExploreOperator]
    public class Where<P> : FilterOperator<P> where P : class, new()
    {
        public ConditionalAtom<P>? Operator { get; set; }

        internal Where()
        {
        }

        protected override void Render()
        {
            if (Operator == null)
                throw new Exception($"Parameter body is mandatory in 'where' filter");

            AppendText("where: ", true);
            Operator.StartWithIndent = false;
            Operator.EndWithLineBreak = true;
            Operator.Render(Output!, IndentationLevel);
        }
    }
}
