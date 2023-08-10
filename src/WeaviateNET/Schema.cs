using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public partial class Schema
    {
        internal WeaviateDB? _connection;
        public async Task Update()
        {
            if (_connection == null) throw new Exception("Empty connection while loading schema");
            var s = await _connection.Client.Schema_dumpAsync();
            if (s == null) throw new Exception("Error while loading the Schema");
            this.Classes = s.Classes;
            this.Maintainer = s.Maintainer;
            this.Name = s.Name;

            foreach (var c in this.Classes)
            {
                c._connection = this._connection;
            }
        }

        public async Task<WeaviateClass<P>> NewClass<P>(WeaviateClass<P> c)
        {
            if (_connection == null) throw new Exception($"Empty connection while creating class '{c.Name}'");
            var flds = typeof(P).GetFields();
            c.Properties = new List<Property>(flds.Length);

            foreach (var f in typeof(P).GetFields())
            {
                // FIXME: not supporting references yet
                c.Properties.Add(Property.Create(f.FieldType, f.Name));
            }
            var nc = await _connection.Client.Schema_objects_createAsync(c);
            

            if (nc == null) throw new Exception($"Error while creating the class '{c.Name}'");
            var ret = new WeaviateClass<P>(nc);
            ret._connection = _connection;
            return ret;
        }

        public async Task<SchemaClusterStatus> ClusterStatus()
        {
            if (_connection == null) throw new Exception($"Empty connection while retrieving cluster status");
            return await _connection.Client.Schema_cluster_statusAsync();
        }
    }
}
