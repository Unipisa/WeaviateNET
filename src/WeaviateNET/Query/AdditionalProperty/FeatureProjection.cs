using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalProperty
{
    public class FeatureProjection : AdditionalField
    {
        public int? Dimensions { get; set; }
        public string? Algorithm { get; set; }
        public int? Perplexity { get; set; }
        public int? LearningRate { get; set; }
        public int? Iterations { get; set; }

        public FeatureProjection()
        {
        }
        protected override void Render()
        {
            if (Dimensions == null)
                throw new Exception("'dimensions' is required in 'featureProjection' additional property");
            AppendLineStartBlock("featureProjection(");
            if (Dimensions != null)
                AppendLine($"dimensions: {Dimensions}");
            if (Algorithm != null)
                AppendLine($"algorithm: {Algorithm}");
            if (Perplexity != null)
                AppendLine($"perplexity: {Perplexity}");
            if (LearningRate != null)
                AppendLine($"learningRate: {LearningRate}");
            if (Iterations != null)
                AppendLine($"iterations: {Iterations}");
            AppendLineBetweenBlocks(") {");
            AppendLineEndBlock("}");
        }
    }
}
