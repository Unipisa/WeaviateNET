using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query
{
    public abstract class ConditionalAtom<P> : QueryBlock where P : class, new()
    {
        internal bool StartWithIndent { get; set; } = true;
        internal bool EndWithLineBreak { get; set; } = true;

        internal ConditionalAtom()
        {
        }
    }
}
