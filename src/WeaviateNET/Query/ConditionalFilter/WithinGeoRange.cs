using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.ConditionalFilter
{
    public class WithinGeoRange<P> : ConditionalAtom<P> where P : class, new()
    {
        public string[]? Path { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public int? MaxDistance { get; set; }

        internal WithinGeoRange()
        {
        }

        protected override void Render()
        {
            if (Path == null)
                throw new Exception($"'path' is mandatory in 'WithinGeoRange' filter");

            if (Latitude == null)
                throw new Exception($"'latitude' is mandatory in 'WithinGeoRange' filter");

            if (Longitude == null)
                throw new Exception($"'longitude' is mandatory in 'WithinGeoRange' filter");

            if (MaxDistance == null)
                throw new Exception($"'distance' is mandatory in 'WithinGeoRange' filter");


            AppendLineStartBlock("{", indent: StartWithIndent);
            AppendLine($"operator: WithinGeoRange,");
            AppendLineStartBlock("valueGeoRange: {");
            AppendLine($"latitude: {JsonConvert.SerializeObject(Latitude)},");
            AppendLine($"longitude: {JsonConvert.SerializeObject(Longitude)}");
            AppendLineEndBlock("},");
            AppendLineStartBlock("distance: {");
            AppendLine($"max: {JsonConvert.SerializeObject(MaxDistance)}");
            AppendLineEndBlock("}");
            AppendLine($"path: {JsonConvert.SerializeObject(Path)}");
            if (EndWithLineBreak)
                AppendLineEndBlock("}");
            else
                AppendTextEndBlock("}");
        }
    }
}
