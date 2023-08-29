using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.SearchOperator
{
    [GetOperator]
    public class BM25<P> : FilterOperator<P> where P : class, new()
    {
        public string? Query { get; set; }
        public string[]? Properties { get; set; }

        internal BM25()
        {
        }

        protected override void Render()
        {
            if (Query == null)
                throw new Exception($"Parameter 'query' is mandatory in BM25 operator");

            AppendLineStartBlock("bm25: {");
            AppendLine($"query: {JsonConvert.SerializeObject(Query)}");
            if (Properties != null)
            {
                if (CheckPropertyNames<P>(Properties) != null)
                    throw new Exception($"Invalid property name in 'BM25' operator");
                AppendLine($"properties: {JsonConvert.SerializeObject(Properties)}");
            }
            AppendLineEndBlock("}");
        }
    }
}
