using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalProperty
{
    public class Rerank : AdditionalField
    {
        public string? Property { get; set; }
        public string? Query { get; set; }

        public Rerank()
        {
        }

        protected override void Render()
        {
            if (Property == null)
                throw new Exception("'property' is required for 'rerank' additional property");
            AppendLineStartBlock("rerank(");
            AppendLine($"property: {JsonConvert.SerializeObject(Property)}");
            if (Query != null)
                AppendLine($"query: {JsonConvert.SerializeObject(Query)}");
            AppendLineBetweenBlocks(") {");
            AppendLine("score");
            AppendLineEndBlock("}");
        }
    }
}
