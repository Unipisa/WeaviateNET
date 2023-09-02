using System.Management.Automation;
using WeaviateNET;

namespace PSWeaviateNET
{
    public class WeaviatePSCmdlet : PSCmdlet
    {
        [Parameter]
        public WeaviateDB? WeaviateDB { get; set; } = null;

        public WeaviateDB Connection
        {
            get
            {
                var wdb = WeaviateDB != null ? WeaviateDB! : 
                    (SessionState.PSVariable.Get("WeaviateDB") == null ? null : (WeaviateDB)SessionState.PSVariable.Get("WeaviateDB").Value);
                if (wdb == null)
                {
                    WriteError(new ErrorRecord(new Exception("WeaviateDB not initialized"), "WeaviateNotInitialized", ErrorCategory.InvalidOperation, null));
                    throw new Exception("Weaviate not initialized");
                }
                return wdb;
            }
        }
    }
}