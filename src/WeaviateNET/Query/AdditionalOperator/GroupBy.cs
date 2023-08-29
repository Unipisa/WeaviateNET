using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalOperator
{
    // In Aggregate Groups and ObjectsPerGroup are ignored
    [GetOperator, AggregateOperator]
    public class GroupBy<P> : FilterOperator<P> where P : class, new() 
    {
        public string[]? Path { get; set; }
        public int? Groups { get; set; }
        public int? ObjectsPerGroup { get; set; }

        public GroupBy()
        {
        }

        protected override void Render()
        {
            if (Path == null)
                throw new Exception($"Parameter 'path' is mandatory in 'groupBy' operator");
            AppendLineStartBlock("groupBy: {");
            AppendLine($"path: {JsonConvert.SerializeObject(Path)}");
            if (Groups.HasValue)
                AppendLine($"groups: {JsonConvert.SerializeObject(Groups)}");
            if (ObjectsPerGroup.HasValue)
                AppendLine($"objectsPerGroup: {JsonConvert.SerializeObject(ObjectsPerGroup)}");
            AppendLineEndBlock("}");
        }
    }
}
