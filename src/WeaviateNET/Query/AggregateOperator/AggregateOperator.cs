using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AggregateOperator
{
    public abstract class AggregateOperator<P> : QueryBlock where P : class, new()
    {
    }
}
