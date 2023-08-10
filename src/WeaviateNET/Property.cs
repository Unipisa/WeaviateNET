using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WeaviateNET
{
    /// <summary>
    /// Note that we assume that DataType is NOT an array.
    /// We map data types in the table https://weaviate.io/developers/weaviate/config-refs/datatypes
    /// </summary>
    public partial class Property
    {

        public static Property Create<T>(string name)
        {
            var ret = new Property() { Name = name };
            ret.SetDataType<T>();
            return ret;
        }

        public static Property Create(Type t, string name)
        {
            var ret = new Property() { Name = name };
            ret.SetDataType(t);
            return ret;
        }

        public static Property CreateRef(string name, params string[] typeNames)
        {
            var ret = new Property() { Name = name };
            ret.SetTypeRefs(typeNames);
            return ret;
        }

        public void SetDataType(Type type)
        {
            DataType = new string[] { WeaviateDataType.MapType(type) };
        }

        public void SetDataType<T>()
        {
            DataType = new string[] { WeaviateDataType.MapType<T>() };
        }

        public void SetTypeRefs(string[] types) {
            DataType = types;
        }
    }
}
