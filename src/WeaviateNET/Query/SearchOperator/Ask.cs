using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.SearchOperator
{
    [GetOperator]
    public class Ask<P> : FilterOperator<P> where P : class, new()
    {
        public string? Question { get; set; }
        public float? Certainty { get; set; }
        public string[]? Properties { get; set; }
        public bool? Rerank { get; set; }

        internal Ask()
        {
        }

        protected override void Render()
        {
            if (Question == null)
                throw new Exception($"Parameter 'question' is mandatory in 'ask' operator");

            AppendLineStartBlock("ask: {");
            AppendLine($"query: {JsonConvert.SerializeObject(Question)}");
            if (Certainty.HasValue)
                AppendLine($"certainty: {JsonConvert.SerializeObject(Certainty)}");
            if (Properties != null)
            {
                if (CheckPropertyNames<P>(Properties) != null)
                    throw new Exception($"Invalid property name in 'hybrid' operator");
                AppendLine($"properties: {JsonConvert.SerializeObject(Properties)}");
            }
            if (Rerank.HasValue)
                AppendLine($"rerank: {JsonConvert.SerializeObject(Rerank)}");
            AppendLineEndBlock("}");
        }
    }
}
