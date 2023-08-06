using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public partial class Class
    {
        internal WeaviateDB? _connection;

        private void CopyFrom(Class src)
        {
            this.MultiTenancyConfig = src.MultiTenancyConfig;
            this.ShardingConfig = src.ShardingConfig;
            this.ReplicationConfig = src.ReplicationConfig;
            this.ModuleConfig = src.ModuleConfig;
            this.Class1 = src.Class1;
            this.Description = src.Description;
            this.InvertedIndexConfig = src.InvertedIndexConfig;
            this.VectorIndexConfig = src.VectorIndexConfig;
            this.VectorIndexType = src.VectorIndexType;
            this.Vectorizer = src.Vectorizer;
        }

        public string Name
        {
            get { return this.Class1; }
            set { this.Class1 = value; }
        }

        public async Task Delete()
        {
            if (_connection == null) throw new Exception($"Error while deleting class '{this.Name}'");
            await _connection.Client.Schema_objects_deleteAsync(this.Name);
        }

        public async Task Save()
        {
            if (_connection == null) throw new Exception($"Error while saving class '{this.Name}'");
            var uc = await _connection.Client.Schema_objects_updateAsync(this.Name, this);
            CopyFrom(uc);
        }

        public async Task Update()
        {
            if (_connection == null) throw new Exception($"Error while loading class '{this.Name}'");
            var uc = await _connection.Client.Schema_objects_getAsync(this.Name);
            CopyFrom(uc);
        }
    }
}
