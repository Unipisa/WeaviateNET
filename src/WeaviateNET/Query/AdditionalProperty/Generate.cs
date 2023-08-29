using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalProperty
{
    public class Generate : AdditionalField
    {
        public string? Prompt { get; set; }

        public Generate() 
        {
        }

        protected override void Render()
        {
            if (Prompt == null)
                throw new Exception("'prompt' is required for 'generate' additional property");
            AppendLineStartBlock("generate(");
            AppendLineStartBlock("singleResult: {");
            AppendLine($"prompt: {JsonConvert.SerializeObject(Prompt)}");
            AppendLineEndBlock("}");
            AppendLineBetweenBlocks(") {");
            AppendLine("singleResult");
            AppendLine("error");
            AppendLineEndBlock("}");
        }
    }
}
