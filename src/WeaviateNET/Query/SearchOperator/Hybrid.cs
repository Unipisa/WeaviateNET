using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET;

namespace WeaviateNET.Query.SearchOperator
{
    [GetOperator]
    public class Hybrid<P> : FilterOperator<P> where P : class, new()
    {
        public string? Query { get; set; }
        public float? Alpha { get; set; }
        public float[]? Vector { get; set; }
        public string[]? Properties { get; set; }
        public string? FusionType { get; set; }

        internal Hybrid()
        {
        }

        protected override void Render()
        {
            if (Query == null)
                throw new Exception($"Parameter 'query' is mandatory in 'hybrid' operator");

            AppendLineStartBlock("hybrid: {");
            AppendLine($"query: {JsonConvert.SerializeObject(Query)}");
            if (Alpha.HasValue)
                AppendLine($"alpha: {JsonConvert.SerializeObject(Alpha)}");
            if (Vector != null)
                AppendLine($"vector: {JsonConvert.SerializeObject(Vector)}");
            if (Properties != null)
            {
                if (CheckPropertyNames<P>(Properties) != null)
                    throw new Exception($"Invalid property name in 'hybrid' operator");
                AppendLine($"properties: {JsonConvert.SerializeObject(Properties)}");
            }
            if (FusionType != null)
                AppendLine($"fusionType: {JsonConvert.SerializeObject(FusionType)}");
            AppendLineEndBlock("}");
        }
    }
}
