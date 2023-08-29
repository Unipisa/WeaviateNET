using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.SearchOperator
{
    [GetOperator, AggregateOperator, ExploreOperator]
    public class NearVector<P> : FilterOperator<P> where P : class, new()
    {
        public float[]? Vector { get; set; }
        public float? Distance { get; set; }
        public float? Certainty { get; set; }

        internal NearVector()
        {
        }

        protected override void Render()
        {
            if (Vector == null)
                throw new Exception($"Parameter vector is mandatory in nearVector operator");

            AppendLineStartBlock("nearVector: {");
            AppendLine($"vector: {JsonConvert.SerializeObject(Vector)}");
            if (Distance.HasValue)
                AppendLine($"distance: {JsonConvert.SerializeObject(Distance)}");
            if (Certainty.HasValue)
                AppendLine($"certainty: {JsonConvert.SerializeObject(Certainty)}");
            AppendLineEndBlock("}");
        }
    }
}
