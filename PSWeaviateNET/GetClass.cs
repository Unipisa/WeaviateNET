using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PSWeaviateNET
{
    [Cmdlet(VerbsCommon.Get, "Class")]
    public class GetClass : WeaviatePSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string? Name { get; set; } = null;

        protected override void ProcessRecord()
        {
            var wdb = Connection;
            WriteObject(wdb.Schema.Classes.Where(n => n.Name == Name).FirstOrDefault());
        }
    }
}
