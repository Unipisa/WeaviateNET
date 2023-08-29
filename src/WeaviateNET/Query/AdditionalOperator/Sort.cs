using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET.Query.AdditionalOperator
{
    public enum Order
    {
        Ascending,
        Descending
    }

    [GetOperator]
    public class Sort<P> : FilterOperator<P> where P : class, new()
    {
        public static Sort<P> Ascending(string[] path) => new Sort<P>() { Path = path, Order = AdditionalOperator.Order.Ascending };
        public static Sort<P> Descending(string[] path) => new Sort<P>() { Path = path, Order = AdditionalOperator.Order.Descending };

        public static Sort<P> MultipleProperties((string, Order)[] properties) {
            var names = Schema.PersistentFieldNames<P>();
            foreach (var property in properties)
            {
                if (!names.Contains(property.Item1))
                    throw new Exception($"Property '{property.Item1}' does not exist in class '{typeof(P).Name}'");
            }
            return new Sort<P>() { PropertiesOrder = properties };
        }

        public string[]? Path { get; set; }
        public Order? Order { get; set; }

        public (string, Order)[]? PropertiesOrder { get; set; }

        public Sort()
        {
        }

        protected override void Render()
        {
            if (Path == null && PropertiesOrder == null)
                throw new Exception($"Parameter 'path' or 'order' is mandatory in 'sort' filter");
            if (Path != null && PropertiesOrder != null)
                throw new Exception($"Only one of parameters 'path' or 'order' should be populated in a 'sort' filter");

            if (Path != null)
            {
                if (Order == null)
                    throw new Exception($"Parameter 'order' is mandatory in 'sort' filter");

                AppendLineStartBlock("sort: {");
                AppendLine($"path: {JsonConvert.SerializeObject(Path)}");
                AppendLine($"order: {(Order == AdditionalOperator.Order.Ascending ? "asc":"desc")}");
                AppendLineEndBlock("}");
                return;
            }

            if (PropertiesOrder != null)
            {
                AppendLineStartBlock("sort: [");
                var first = true;
                foreach (var (p, o) in PropertiesOrder)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        AppendLine(",", indent: false);
                    }
                    AppendLineStartBlock("{");
                    AppendLine($"path: {JsonConvert.SerializeObject(p)}");
                    AppendLine($"order: {(o == AdditionalOperator.Order.Ascending ? "asc" : "desc")}");
                    AppendLineEndBlock("}");

                }
                AppendLineEndBlock("]");
            }
        }
    }
}
