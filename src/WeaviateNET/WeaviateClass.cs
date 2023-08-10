using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public partial class WeaviateClassBase
    {
        internal WeaviateDB? _connection;

        internal void CopyFrom(WeaviateClassBase src)
        {
            this.MultiTenancyConfig = src.MultiTenancyConfig;
            this.ShardingConfig = src.ShardingConfig;
            this.ReplicationConfig = src.ReplicationConfig;
            this.ModuleConfig = src.ModuleConfig;
            this.Class1 = src.Class1;
            this.Description = src.Description;
            this.InvertedIndexConfig = src.InvertedIndexConfig;
            this.Properties = src.Properties;
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

        public async Task AddProperty(Property p)
        {
            if (_connection == null) throw new Exception($"Empty connection while adding property '{p.Name}' to class '{this.Name}'");
            await _connection.Client.Schema_objects_properties_addAsync(this.Name, p);
            await this.Update();
        }

        public async Task Add<P>(WeaviateNET.WeaviateObject<P> obj, string consistency_level="QUORUM")
        {
            if (_connection == null) throw new Exception($"Empty connection while adding object to class '{this.Name}'");
            obj.Class = this.Name;
            var o = await _connection.Client.Objects_createAsync(obj, consistency_level);
        }

        #region Shards
        public async Task<ICollection<ShardStatusGetResponse>> GetShardsStatus()
        {
            if (_connection == null) throw new Exception($"Error while getting shards status for class '{this.Name}'");
            return await _connection.Client.Schema_objects_shards_getAsync(this.Name);
        }

        public async Task SetShardStatus(string shardName, ShardStatus status)
        {
            if (_connection == null) throw new Exception($"Error while setting shards status for class '{this.Name}'");
            await _connection.Client.Schema_objects_shards_updateAsync(this.Name, shardName, status);
            await this.Update();
        }
        #endregion

        #region Tenants
        public async Task<ICollection<Tenant>> GetTenants()
        {
            if (_connection == null) throw new Exception($"Error while getting tenants status for class '{this.Name}'");
            return await _connection.Client.Tenants_getAsync(this.Name);
        }

        public async Task AddTenants(IEnumerable<Tenant> tenants)
        {
            if (_connection == null) throw new Exception($"Error while adding tenants status for class '{this.Name}'");
            await _connection.Client.Tenants_createAsync(this.Name, tenants);
            await this.Update();
        }

        public async Task RemoveTenants(IEnumerable<string> tenants)
        {
            if (_connection == null) throw new Exception($"Error while removing tenants status for class '{this.Name}'");
            await _connection.Client.Tenants_deleteAsync(this.Name, tenants);
            await this.Update();
        }
        #endregion
    }

    public class WeaviateClass<P> : WeaviateClassBase
    {
        public WeaviateClass() : base()
        {
        }

        public WeaviateClass(WeaviateClassBase b) : this()
        {
            this.CopyFrom(b);
        }
    }
}
