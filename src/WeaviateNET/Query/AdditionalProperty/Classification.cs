using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalProperty
{
    public class Classification : AdditionalField
    {
        public Classification()
        {
        }

        protected override void Render()
        {
            AppendLineStartBlock("classification {");
            AppendLine("basedOn");
            AppendLine("classifiedFields");
            AppendLine("completed");
            AppendLine("id");
            AppendLine("scope");
            AppendLineEndBlock("}");
        }
    }
}
