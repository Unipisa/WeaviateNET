using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET;

namespace PSWeaviateNET
{
    [Cmdlet(VerbsCommon.Find, "WeaviateObjects")]
    public class QueryWeaviate : WeaviatePSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string? Query { get; set; } = null;

        protected override void ProcessRecord()
        {
            var wdb = Connection;

            var q = new GraphQLQuery();
            q.Query = Query!;
            var r = wdb.Schema.RawQuery(q);
            r.Wait();

            WriteObject(r.Result.Data);
            if (r.Result.Errors != null)
            {
                foreach (var e in r.Result.Errors)
                {
                    WriteError(new ErrorRecord(new Exception(e.Message), "WeaviateError", ErrorCategory.InvalidOperation, e));
                }
            }
        }
    }
}
