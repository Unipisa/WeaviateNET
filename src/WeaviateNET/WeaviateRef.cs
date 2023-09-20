using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public class WeaviateRef
    {
        public string TargetClassName { get; set; }
        public Guid TargetId { get; set; }

        public WeaviateRef(string targetClassName, Guid id)
        {
            TargetClassName = targetClassName;
            TargetId = id;
        }

        public override string ToString()
        {
            return $"weaviate://localhost/{TargetClassName}/{TargetId}";
        }
    }
}
