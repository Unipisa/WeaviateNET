using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.SearchOperator
{
    public enum GroupType
    {
        Closest,
        Merge
    }

    [GetOperator]
    public class Group<P> : FilterOperator<P> where P : class, new()
    {
        public GroupType? Type { get; set; }
        public float? Force { get; set; }

        internal Group()
        {
        }

        protected override void Render()
        {
            if (!Type.HasValue)
                throw new Exception($"Parameter 'query' is mandatory in 'hybrid' operator");

            AppendLineStartBlock("group: {");
            AppendLine($"type: {(Type! == GroupType.Closest ? "closest" : "merge")}");
            if (Force.HasValue)
                AppendLine($"force: {JsonConvert.SerializeObject(Force)}");
            AppendLineEndBlock("}");
        }
    }
}
