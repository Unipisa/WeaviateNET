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
        }

        public async Task<Class> NewClass(Class c)
        {
            if (_connection == null) throw new Exception($"Empty connection while creating class '{c.Name}'");
            var nc = await _connection.Client.Schema_objects_createAsync(c);
            if (nc == null) throw new Exception($"Error while creating the class '{c.Name}'");
            nc._connection = _connection;
            return nc;
        }
    }
}
