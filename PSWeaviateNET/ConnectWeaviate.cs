using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET;

namespace PSWeaviateNET
{
    public class WeaviateConfiguration
    {
        public static WeaviateConfiguration? FromJson(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<WeaviateConfiguration>(json);
        }

        public string? WeaviateEndpoint { get; set; }
        public string? WeaviateApiKey { get; set; }
    }

    [Cmdlet(VerbsCommunications.Connect, "Weaviate")]
    public class ConnectWeaviate : WeaviatePSCmdlet
    {
        [Parameter]
        public string? ConfigFile { get; set; }

        [Parameter]
        public WeaviateConfiguration Config { get; set; } = null!;

        protected override void ProcessRecord()
        {
            if (ConfigFile != null)
            {
                var json = System.IO.File.ReadAllText(ConfigFile);
                Config = WeaviateConfiguration.FromJson(json)!;
            }
            else if (Config == null)
            {
                throw new Exception("Either ConfigFile or Config must be set");
            }

            var weaviateDb = new WeaviateDB(Config.WeaviateEndpoint!, Config.WeaviateApiKey);
            weaviateDb.Schema.Update().Wait();

            SessionState.PSVariable.Set("WeaviateDB", weaviateDb);
            WriteObject(weaviateDb);
        }
    }
}
