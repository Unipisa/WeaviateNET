using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query
{
    public abstract class FilterOperator<P> : QueryBlock where P : class
    {
        internal FilterOperator()
        {

        }
    }
}
