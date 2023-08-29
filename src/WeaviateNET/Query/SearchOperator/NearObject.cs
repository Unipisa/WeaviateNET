using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.SearchOperator
{
    [GetOperator, AggregateOperator, ExploreOperator]
    public class NearObject<P> : FilterOperator<P> where P : class, new()
    {
        public Guid? Id { get; set; }
        public string? Beacon { get; set; }
        public float? Distance { get; set; }
        public float? Certainty { get; set; }

        internal NearObject()
        {
        }

        protected override void Render()
        {
            if (Id == null)
                throw new Exception($"Parameter 'id' is mandatory in 'nearObject' operator");

            if (Beacon == null)
                throw new Exception($"Parameter 'beacon' is mandatory in 'nearObject' operator");

            AppendLineStartBlock("nearObject: {");
            AppendLine($"id: {JsonConvert.SerializeObject(Id)}");
            AppendLine($"beacon: {JsonConvert.SerializeObject(Beacon)}");
            if (Distance.HasValue)
                AppendLine($"distance: {JsonConvert.SerializeObject(Distance)}");
            if (Certainty.HasValue)
                AppendLine($"certainty: {JsonConvert.SerializeObject(Certainty)}");
            AppendLineEndBlock("}");
        }
    }
}
