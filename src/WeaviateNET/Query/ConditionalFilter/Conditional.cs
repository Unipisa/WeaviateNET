using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.ConditionalOperator
{
    public class Conditional<P> : ConditionalAtom<P> where P : class, new()
    {
        public static Conditional<P> And(params ConditionalAtom<P>[] ops) => new Conditional<P>("And") { Operands = ops };
        public static Conditional<P> Or(params ConditionalAtom<P>[] ops) => new Conditional<P>("Or")  { Operands = ops };

        internal string OperatorName { get; set; }
        public ConditionalAtom<P>[]? Operands { get; set; }

        internal Conditional(string operatorName)
        {
            OperatorName = operatorName;
        }

        protected override void Render()
        {
            if (Operands == null || Operands.Length == 0)
                throw new Exception($"'operands' are mandatory in '{OperatorName}' filter");

            AppendLineStartBlock("{", indent: StartWithIndent);
            AppendLine($"operator: {OperatorName},");
            AppendText("operands: [", true);
            var first = true;
            foreach (var o in Operands)
            {
                o.StartWithIndent = !first;
                if (first)
                {
                    first = false;
                }
                else
                {
                    AppendLine(",", indent: false);
                }
                o.EndWithLineBreak = false;
                o.Render(Output!, IndentationLevel);
            }
            AppendLine("]", indent: false);
            if (EndWithLineBreak)
              AppendLineEndBlock("}");
            else
              AppendTextEndBlock("}");
        }
    }
}
