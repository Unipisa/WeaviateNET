using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public partial class WeaviateObject<P>
    {
        internal WeaviateClassBase? classType;

        public WeaviateObject() {
#if AUTO_GENERATE_OID
            // Better have an id for each object
            // You can still nullate the field, however the AUTO_GENERATE_OID compiler directive enables this behaviour
            this.Id = Guid.NewGuid();
#endif
        }

        public WeaviateObject(WeaviateClassBase classType, Guid id=new Guid()) : base()
        {
            this.classType = classType;
            this.Class = classType.Name;
            if (id != Guid.Empty)
            {
                this.Id = id;
            }
        }

        //public T GetValue<T>(string property)
        //{
        //    if (classType == null) { throw new Exception("Object without a class!"); }
        //    var p = classType.Properties.FirstOrDefault(p => p.Name == property);
        //    if (p == null) { throw new Exception($"Class '{classType.Name}' does not contains a definition for property '{property}'"); }
        //    if (p.DataType.Count == 1)
        //    {
        //        if (WeaviateDataType.MapType<T>() != p.DataType.First()) {
        //            throw new Exception("Invalid type"); 
        //        }

        //    }
        //}
    }
}
