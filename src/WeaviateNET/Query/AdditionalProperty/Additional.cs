using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalProperty
{
    public class Additional : AdditionalField
    {
        public static AdditionalField Id { get { return new Additional("id"); } }
        public static AdditionalField Vector { get { return new Additional("vector"); } }
        public static AdditionalField CreationTimeUnix { get { return new Additional("creationTimeUnix"); } }
        public static AdditionalField LastUpdateTimeUnix { get { return new Additional("lastUpdateTimeUnix"); } }
        public static AdditionalField Distance { get { return new Additional("distance"); } }
        public static AdditionalField Certainty { get { return new Additional("certainty"); } }
        public static AdditionalField Rerank(string property, string? query = null) => new Rerank() { Property = property, Query = query };
        public static AdditionalField Generate(string prompt) => new Generate() { Prompt = prompt };
        public static AdditionalField Classification { get { return new Classification(); } }
        public static AdditionalField FeatureProjection(int dimensions, string? algorithm = null, int? perplexity = null, int? learningRate = null, int? iterations = null)
        {
            return new FeatureProjection()
            {
                Dimensions = dimensions,
                Algorithm = algorithm,
                Perplexity = perplexity,
                LearningRate = learningRate,
                Iterations = iterations
            };
        }
        public static AdditionalField Group<P>(string[] properties) where P : class, new() => new Group<P>() { Properties = properties };

        private string _name;

        public Additional(string name)
        {
            _name = name;
        }

        protected override void Render()
        {
            AppendLine(_name);
        }
    }
}
