using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Modules
{
    public abstract class VectorizerAttribute : Attribute
    {
    }

    #region AWS
    [AttributeUsage(AttributeTargets.Class)]
    public class AWSVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.AWSClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? model { get; set; }
        public string region { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AWSVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.AWSClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Cohere
    [AttributeUsage(AttributeTargets.Class)]
    public class CohereVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.CohereClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? model { get; set; }
        public string? truncate { get; set; }
        public string? baseUrl { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CohereVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.CohereClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Palm
    [AttributeUsage(AttributeTargets.Class)]
    public class PalmVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.PalmClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? projectId { get; set; }
        public string? apiEndpoint { get; set; }
        public string? modelId { get; set; }
        public string? titleProperty { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PalmVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.PalmClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region HuggingFace
    [AttributeUsage(AttributeTargets.Class)]
    public class HuggingFaceVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.HuggingFaceClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? model { get; set; }
        public string? passageModel { get; set; }
        public string? queryModel { get; set; }
        public string? endpointURL { get; set; }
        public Text2Vec.HuggingFaceClassModParamsOptions? options { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class HuggingFaceVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.HuggingFaceClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region JinaAI 
    [AttributeUsage(AttributeTargets.Class)]
    public class JinaAIVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.JinaAIClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? model { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class JinaAIVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.JinaAIClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region OpenAI
    [AttributeUsage(AttributeTargets.Class)]
    public class OpenAIVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.OpenAIClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? model { get; set; }
        public int dimensions { get; set; }
        public string? modelVersion { get; set; }
        public string? type { get; set; }
        public string? baseUrl { get; set; }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class OpenAIVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.OpenAIClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Contextionary 
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextionaryVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.ContextionaryClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ContextionaryVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.ContextionaryClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Transformers 
    [AttributeUsage(AttributeTargets.Class)]
    public class TransformersVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.TransformersClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string? poolingStrategy { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class TransformersVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.TransformersClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region GPT4All 
    [AttributeUsage(AttributeTargets.Class)]
    public class GPT4AllVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.GPT4AllClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class GPT4AllVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Text2Vec.GPT4AllClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Img Neural 
    [AttributeUsage(AttributeTargets.Class)]
    public class NeuralVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Img2Vec.NeuralClassModCfg.ModuleName;
        public string[]? imageFields { get; set; }
    }
    #endregion

    #region Multi Clip 
    [AttributeUsage(AttributeTargets.Class)]
    public class ClipVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Multi2Vec.ClipClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string[]? textFields { get; set; }
        public string[]? imageFields { get; set; }
        public Multi2Vec.ClipClassModParamsWeights? weights { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ClipVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Multi2Vec.ClipClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Multi Bind 
    [AttributeUsage(AttributeTargets.Class)]
    public class BindVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Multi2Vec.BindClassModCfg.ModuleName;
        public bool vectorizeClassName { get; set; } = true;
        public string[]? textFields { get; set; }
        public string[]? imageFields { get; set; }
        public string[]? audioFields { get; set; }
        public string[]? videoFields { get; set; }
        public string[]? depthFields { get; set; }
        public string[]? thermalFields { get; set; }
        public string[]? IMUFields { get; set; }
        public Multi2Vec.BindClassModParamsWeights? weights { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class BindVectorizerPropertyAttribute : VectorizerAttribute
    {
        public const string ModuleName = Multi2Vec.BindClassModCfg.ModuleName;
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }
    #endregion

    #region Ref Centroid 
    [AttributeUsage(AttributeTargets.Class)]
    public class CentroidVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = "ref2vec-centroid";
    }
    #endregion

    #region Reranker Cohere 
    [AttributeUsage(AttributeTargets.Class)]
    public class RerankerCohereVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Reranker.RerankerCohereClassModCfg.ModuleName;
        public string? model { get; set; }
    }
    #endregion

    #region Reranker Transformers 
    [AttributeUsage(AttributeTargets.Class)]
    public class RerankerTransformersVectorizerClassAttribute : VectorizerAttribute
    {
        public const string ModuleName = Reranker.RerankerTransformersClassModCfg.ModuleName;
    }
    #endregion
}
