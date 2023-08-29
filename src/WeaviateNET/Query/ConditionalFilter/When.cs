using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.ConditionalOperator
{
    public class When<P, V> : ConditionalAtom<P> where P : class, new()
    {
        private static Type[] allowedTypesComp = new Type[] { typeof(string), typeof(int), typeof(float), typeof(bool), typeof(DateTime) };
        private static Type[] allowedTypeText = new Type[] { typeof(string) };
        private static Type[] allowedTypeAnyText = new Type[] { typeof(string), typeof(string[]) };

        private static When<P, V> Op<V>(string opName, string property, V value, Type[] allowedTypes)
        {
            if (!allowedTypes.Contains(typeof(V)))
                throw new ArgumentException($"Type '{typeof(V)}' is not allowed for '{opName}' filter");

            if (!Schema.PersistentFieldNames<P>().Contains(property))
                throw new ArgumentException($"Property '{property}' does not exist in class '{typeof(P)}'");

            var ret = new When<P, V>();
            ret.Path = new string[] { property };
            ret.Operator = opName;
            ret.Value = value;
            return ret;
        }

        private static When<P, V> Op<V>(string opName, string[] path, V value, Type[] allowedTypes)
        {
            if (!allowedTypes.Contains(typeof(V)))
            {
                throw new ArgumentException($"Type '{typeof(V)}' is not allowed for '{opName}' filter");
            }
            var ret = new When<P, V>();
            ret.Path = path;
            ret.Operator = opName;
            ret.Value = value;
            return ret;
        }

        public static When<P, V> Equal(string property, V value) => Op("Equal", property, value, allowedTypesComp);
        public static When<P, V> Equal(string[] path, V value) => Op("Equal", path, value, allowedTypesComp);

        public static When<P, V> NotEqual(string property, V value) => Op("NotEqual", property, value, allowedTypesComp);
        public static When<P, V> NotEqual(string[] path, V value) => Op("NotEqual", path, value, allowedTypesComp);

        public static When<P, V> GreaterThan(string property, V value) => Op("GreaterThan", property, value, allowedTypesComp);
        public static When<P, V> GreaterThan(string[] path, V value) => Op("GreaterThan", path, value, allowedTypesComp);

        public static When<P, V> GreaterThanEqual(string property, V value) => Op("GreaterThanEqual", property, value, allowedTypesComp);
        public static When<P, V> GreaterThanEqual(string[] path, V value) => Op("GreaterThanEqual", path, value, allowedTypesComp);

        public static When<P, V> LessThan(string property, V value) => Op("LessThan", property, value, allowedTypesComp);
        public static When<P, V> LessThan(string[] path, V value) => Op("LessThan", path, value, allowedTypesComp);

        public static When<P, V> LessThanEqual(string property, V value) => Op("LessThanEqual", property, value, allowedTypesComp);
        public static When<P, V> LessThanEqual(string[] path, V value) => Op("LessThanEqual", path, value, allowedTypesComp);

        public static When<P, V> Like(string property, V value) => Op("Like", property, value, allowedTypeText);
        public static When<P, V> Like(string[] path, V value) => Op("Like", path, value, allowedTypeText);

        public static When<P, V> ContainsAny(string property, V value) => Op("ContainsAny", property, value, allowedTypeAnyText);
        public static When<P, V> ContainsAny(string[] path, V value) => Op("ContainsAny", path, value, allowedTypeAnyText);

        public static When<P, V> ContainsAll(string property, V value) => Op("ContainsAll", property, value, allowedTypeAnyText);
        public static When<P, V> ContainsAll(string[] path, V value) => Op("ContainsAll", path, value, allowedTypeAnyText);

        public static When<P, bool> IsNull(string property) => Op<bool>("IsNull", property, true, new Type[] { typeof(bool) });
        public static When<P, bool> IsNull(string[] path) => Op<bool>("IsNull", path, true, new Type[] { typeof(bool) });

        public static When<P, bool> IsNotNull(string property) => Op<bool>("IsNull", property, false, new Type[] { typeof(bool) });
        public static When<P, bool> IsNotNull(string[] path) => Op<bool>("IsNull", path, false, new Type[] { typeof(bool) });

        public string[]? Path { get; set; }
        public string? Operator { get; set; }
        public V? Value { get; set; }

        internal When()
        {
        }

        protected override void Render()
        {
            if (Operator == null)
                throw new Exception($"'operator' is mandatory in filter");

            if (Path == null)
                throw new Exception($"'path' is mandatory in '{Operator}' filter");

            if (Value == null)
                throw new Exception($"'value' is mandatory in '{Operator}' filter");

            AppendLineStartBlock("{", indent: StartWithIndent);
            AppendLine($"operator: {Operator},");
            AppendLine($"path: {JsonConvert.SerializeObject(Path)},");
            AppendLine($"{WeaviateDataType.SearchValueType<V>()}: {JsonConvert.SerializeObject(Value)},");
            if (EndWithLineBreak)
                AppendLineEndBlock("}");
            else
                AppendTextEndBlock("}");

        }
    }
}
