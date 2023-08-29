using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalProperty
{
    public class Group<P> : AdditionalField where P : class, new()
    {
        public string[]? Properties { get; set; }

        public Group()
        {
        }

        protected override void Render()
        {
            if (Properties == null)
                throw new Exception($"Parameter 'properties' is mandatory in 'group' field");
            AppendLineStartBlock("group: {");
            AppendLine("id");
            AppendLine("groupedBy { value path }");
            AppendLine("count");
            AppendLine("maxDistance");
            AppendLine("minDistance");
            AppendLineStartBlock("hits {");
            AppendLine($"{JsonConvert.SerializeObject(Properties)}");
            AppendLineStartBlock("_additional");
            AppendLine("id");
            AppendLine("vector");
            AppendLine("distance");
            AppendLineEndBlock("}");
            AppendLineEndBlock("}");
            AppendLineEndBlock("}");
        }
    }
}
