using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PSWeaviateNET
{
    [Cmdlet(VerbsCommon.Get, "Schema")]
    public class GetSchema : WeaviatePSCmdlet
    {
        protected override void ProcessRecord()
        {
            var wdb = Connection;

            wdb.Schema.Update().Wait();
            WriteObject(wdb.Schema);
        }
    }
}
