using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeaviateNET.Modules.Text2Vec
{
    #region text2vec-aws
    public class AWSClassModParams
    {
        public string? model { get; set; }
        public string region { get; set; }
        public bool vectorizeClassName { get; set; } = true;
    }

    public class AWSClassModCfg
    {
        public const string ModuleName = "text2vec-aws";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public AWSClassModParams? Parameters { get; set; }
    }

    public class AWSPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class AWSPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(AWSClassModCfg.ModuleName)]
        public AWSPropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-cohere
    public class CohereClassModParams
    {
        public string? model { get; set; }
        public string? truncate { get; set; }
        public string? baseUrl { get; set; }
        public bool vectorizeClassName { get; set; } = true;
    }

    public class CohereClassModCfg
    {
        public const string ModuleName = "text2vec-cohere";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public CohereClassModParams? Parameters { get; set; }
    }

    public class CoherePropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class CoherePropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(CohereClassModCfg.ModuleName)]
        public CoherePropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-palm
    public class PalmClassModParams
    {
        public string? projectId { get; set; }
        public string? apiEndpoint { get; set; }
        public string? modelId { get; set; }
        public string? titleProperty { get; set; }
        public bool vectorizeClassName { get; set; } = true;
    }

    public class PalmClassModCfg
    {
        public const string ModuleName = "text2vec-palm";

        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public PalmClassModParams? Parameters { get; set; }
    }

    public class PalmPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class PalmPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(PalmClassModCfg.ModuleName)]
        public PalmPropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-huggingface

    public class HuggingFaceClassModParamsOptions
    {
        public bool waitForModel { get; set; }
        public bool useGPU { get; set; }
        public bool useCache { get; set; }
    }

    public class HuggingFaceClassModParams
    {
        public string? model { get; set; }
        public string? passageModel { get; set; }
        public string? queryModel { get; set; }
        public string? endpointURL { get; set; }
        public HuggingFaceClassModParamsOptions? options { get; set; }
        public bool vectorizeClassName { get; set; } = true;
    }

    public class HuggingFaceClassModCfg
    {
        public const string ModuleName = "text2vec-huggingface";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public HuggingFaceClassModParams? Parameters { get; set; }
    }

    public class HuggingFacePropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class HuggingFacePropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(HuggingFaceClassModCfg.ModuleName)]
        public HuggingFacePropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-jinaai
    public class JinaAIClassModParams
    {
        public string? model { get; set; }
        public bool vectorizeClassName { get; set; } = true;
    }

    public class JinaAIClassModCfg
    {
        public const string ModuleName = "text2vec-jinaai";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public JinaAIClassModParams? Parameters { get; set; }
    }

    public class JinaAIPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class JinaAIPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(JinaAIClassModCfg.ModuleName)]
        public JinaAIPropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-openai
    public class OpenAIClassModParams
    {
        public bool vectorizeClassName { get; set; } = true;
        public string? model { get; set; }
        public int? dimensions { get; set; }
        public string? modelVersion { get; set; }
        public string? type { get; set; }
        public string? baseUrl { get; set; }
        public string? resourceName { get; set; }
        public string? deploymentId { get; set; }
    }

    public class OpenAIClassModCfg
    {
        public const string ModuleName = "text2vec-openai";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public OpenAIClassModParams? Parameters { get; set; }
    }

    public class OpenAIPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class OpenAIPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(OpenAIClassModCfg.ModuleName)]
        public OpenAIPropertyModParams? Parameters { get; set; }
    }

    #endregion

    #region text2vec-contextionary
    public class ContextionaryClassModParams
    {
        public bool vectorizeClassName { get; set; } = true;
    }

    public class ContextionaryClassModCfg
    {
        public const string ModuleName = "text2vec-contextionary";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public ContextionaryClassModParams? Parameters { get; set; }
    }

    public class ContextionaryPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class ContextionaryPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(ContextionaryClassModCfg.ModuleName)]
        public ContextionaryPropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-transformers
    public class TransformersClassModParams
    {
        public bool vectorizeClassName { get; set; } = true;
        public string? poolingStrategy { get; set; }
    }

    public class TransformersClassModCfg
    {
        public const string ModuleName = "text2vec-transformers";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public TransformersClassModParams? Parameters { get; set; }
    }

    public class TransformersPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class TransformersPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(TransformersClassModCfg.ModuleName)]
        public TransformersPropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region text2vec-gpt4all
    public class GPT4AllClassModParams
    {
        public bool vectorizeClassName { get; set; } = true;
    }

    public class GPT4AllClassModCfg
    {
        public const string ModuleName = "text2vec-gpt4all";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public GPT4AllClassModParams? Parameters { get; set; }
    }

    public class GPT4AllPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class GPT4AllPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(GPT4AllClassModCfg.ModuleName)]
        public GPT4AllPropertyModParams? Parameters { get; set; }
    }
    #endregion
}

namespace WeaviateNET.Modules.Img2Vec
{
    #region img2vec-neural
    public class NeuralClassModParams
    {
        public string[]? imageFields { get; set; }
    }

    public class NeuralClassModCfg
    {
        public const string ModuleName = "img2vec-neural";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public NeuralClassModParams? Parameters { get; set; }
    }
    #endregion
}

namespace WeaviateNET.Modules.Multi2Vec
{
    #region multi2vec-clip

    public class ClipClassModParamsWeights
    {
        public float[]? textFields { get; set; }
        public float[]? imageFields { get; set; }
    }

    public class ClipClassModParams
    {
        public bool vectorizeClassName { get; set; } = true;
        public string[]? textFields { get; set; }
        public string[]? imageFields { get; set; }
        public ClipClassModParamsWeights? weights { get; set; }
    }

    public class ClipClassModCfg
    {
        public const string ModuleName = "multi2vec-clip";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public ClipClassModParams? Parameters { get; set; }
    }

    public class ClipPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class ClipPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(ClipClassModCfg.ModuleName)]
        public ClipPropertyModParams? Parameters { get; set; }
    }
    #endregion

    #region multi2vec-bind

    public class BindClassModParamsWeights
    {
        public float[]? textFields { get; set; }
        public float[]? imageFields { get; set; }
        public float[]? audioFields { get; set; }
        public float[]? videoFields { get; set; }
        public float[]? depthFields { get; set; }
        public float[]? thermalFields { get; set; }
        public float[]? IMUFields { get; set; }
    }

    public class BindClassModParams
    {
        public bool vectorizeClassName { get; set; } = true;
        public string[]? textFields { get; set; }
        public string[]? imageFields { get; set; }
        public string[]? audioFields { get; set; }
        public string[]? videoFields { get; set; }
        public string[]? depthFields { get; set; }
        public string[]? thermalFields { get; set; }
        public string[]? IMUFields { get; set; }
        public BindClassModParamsWeights? weights { get; set; }
    }

    public class BindClassModCfg
    {
        public const string ModuleName = "multi2vec-bind";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public BindClassModParams? Parameters { get; set; }
    }

    public class BindPropertyModParams
    {
        public bool vectorizePropertyName { get; set; } = false;
        public bool skip { get; set; } = false;
    }

    public class BindPropertyModCfg
    {
        [Newtonsoft.Json.JsonProperty(ClipClassModCfg.ModuleName)]
        public BindPropertyModParams? Parameters { get; set; }
    }
    #endregion
}

namespace WeaviateNET.Modules.Reranker
{
    #region reranker-cohere
    public class RerankerCohereClassModParams
    {
        public string? model { get; set; }
    }

    public class RerankerCohereClassModCfg
    {
        public const string ModuleName = "reranker-cohere";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public RerankerCohereClassModParams? Parameters { get; set; }
    }
    #endregion

    #region reranker-transformers
    public class RerankerTransformersClassModParams
    {
    }

    public class RerankerTransformersClassModCfg
    {
        public const string ModuleName = "reranker-transformers";
        [Newtonsoft.Json.JsonProperty(ModuleName)]
        public RerankerTransformersClassModParams? Parameters { get; set; }
    }
    #endregion
}