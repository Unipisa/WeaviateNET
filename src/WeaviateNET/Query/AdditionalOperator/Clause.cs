using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalOperator
{

    public enum ConsistencyLevel
    {
        ONE,
        QUORUM,
        ALL
    }

    [GetOperator, AggregateOperator]
    public class Clause<P, V> : FilterOperator<P> where P : class, new()
    {
        private static Clause<P, V> Cl<V>(string name, V value)
        {
            return new Clause<P, V>(name)
            {
                Value = value
            };
        }

        public static Clause<P, int> Limit(int v) => Cl<int>("limit", v);
        public static Clause<P, int> Offset(int v) => Cl<int>("offset", v);
        public static Clause<P, int> Autocut(int v) => Cl<int>("autocut", v);
        public static Clause<P, Guid> After(Guid v) => Cl<Guid>("after", v);
        public static Clause<P, string> Tenant(string v) => Cl<string>("tenant", v);
        public static Clause<P, ConsistencyLevel> Consistency(ConsistencyLevel v) => Cl<ConsistencyLevel>("consistencyLevel", v);
        public static Clause<P, int> ObjectLimit(int v) => Cl<int>("objectLimit", v);


        internal string? Name { get; set; }

        public V? Value { get; set; }

        internal Clause(string? name)
        {
            Name = name;
        }

        protected override void Render()
        {
            if (Name == null)
                throw new Exception($"Invalid clause");

            if (Value == null)
                throw new Exception($"Value is mandatory in '{Name}' clause");

            if (typeof(V) == typeof(ConsistencyLevel))
            {
                ConsistencyLevel v = ConsistencyLevel.QUORUM;
                if (Value.Equals(ConsistencyLevel.ONE))
                    v = ConsistencyLevel.ONE;
                else if (Value.Equals(ConsistencyLevel.ALL))
                    v = ConsistencyLevel.ALL;
                else if (Value.Equals(ConsistencyLevel.QUORUM))
                    v = ConsistencyLevel.QUORUM;
                else
                    throw new Exception($"Invalid value for '{Name}' clause");
                AppendLine($"{Name}: {Enum.ToObject(typeof(ConsistencyLevel), v)}");
            }
            else
            {
                AppendLine($"{Name}: {JsonConvert.SerializeObject(Value)}");
            }
        }
    }
}
