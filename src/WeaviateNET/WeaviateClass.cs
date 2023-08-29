using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET.Query.AdditionalOperator;
using WeaviateNET.Query.ConditionalOperator;

namespace WeaviateNET
{
    public partial class WeaviateClassBase
    {
        [JsonIgnore]
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

    public class WeaviateClass<P> : WeaviateClassBase where P : class, new()
    {
        internal WeaviateClass() : base()
        {
        }

        internal WeaviateClass(WeaviateClassBase b) : this()
        {
            this.CopyFrom(b);
        }

        public WeaviateObject<P> Create()
        {
            var ret = new WeaviateObject<P>();
            ret.Class = this.Name;
            ret.classType = this;
            return ret;
        }

        public WeaviateObject<P> Create (P data)
        {
            var ret = this.Create();
            ret.Properties = data;
            return ret;
        }

        public async Task Add(WeaviateObject<P> obj, string consistency_level = "QUORUM")
        {
            if (_connection == null) throw new Exception($"Empty connection while adding object to class '{this.Name}'");
            obj.Class = this.Name;
            var o = await _connection.Client.Objects_createAsync(obj, consistency_level);
            obj.CopyFrom(o);
        }

        public async Task<ICollection<WeaviateObject<P>>> Add(ICollection<WeaviateObject<P>> objects, string consistency_level = "QUORUM")
        {
            if (_connection == null) throw new Exception($"Empty connection while batch adding objects to class '{this.Name}'");
            var b = new Body<P>();
            b.Objects = objects;
            var ret = await _connection.Client.Batch_objects_createAsync(b, consistency_level);
            var idx = new Dictionary<Guid, ObjectsGetResponse<P>>();
            foreach (var r in ret) {
                if (r.Result.Status == Result2Status.SUCCESS)
                {
                    if (r.Id.HasValue) 
                    { 
                        idx.Add((Guid)r.Id, r);
                    }
                }
            }
            var lret = new List<WeaviateObject<P>>();
            foreach (var o in objects)
            {
                if (o.Id.HasValue && idx.ContainsKey((Guid)o.Id))
                {
                    o.CopyFrom(idx[(Guid)o.Id]);
                    lret.Add(o);
                }
            }
            return lret;
        }

        public async Task<BatchDeleteResponse> DeleteObjects(BatchDelete q, string consistency_level = "QUORUM", string? tenant = null)
        {
            if (_connection == null) throw new Exception($"Empty connection while batch deleting objects to class '{this.Name}'");
            return await _connection.Client.Batch_objects_deleteAsync(q, consistency_level, tenant);
        }

        public async Task<WeaviateObject<P>?> Get(Guid id, string consistency_level = "QUORUM", string? include = null, string? node_name = null, string? tenant = null)
        {
            if (_connection == null) throw new Exception($"Empty connection while fetching object '{id}'");
            var ret = await _connection.Client.Objects_class_getAsync<P>(this.Name, id, include, consistency_level, node_name, tenant);
            if (ret != null)
                ret.classType = this;
            return ret;
        }

        public async Task<ObjectsListResponse<P>> ListObjects(long limit = 25, string? after=null, long? offset=null, string? include=null, string? sort=null, string? order=null, string? tenant=null)
        {
            // Apparently if the parameter limit is omitted you will get 0 results (tested with curl).
            if (_connection == null) throw new Exception($"Empty connection while listing objects of class '{this.Name}'");
            var ret = await _connection.Client.Objects_listAsync<P>(after, offset, limit, include, sort, order, this.Name, tenant);
            foreach (var o in ret.Objects)
            {
                o.classType = this;
            }
            return ret;
        }

        public Query.Get<P> CreateGetQuery(bool selectall=true) => new Query.Get<P>(this, selectall);
        public Query.Aggregate<P> CreateAggregateQuery() => new Query.Aggregate<P>(this);

        public async Task<int> CountObjects()
        {
            if (_connection == null) throw new Exception($"Empty connection while counting objects of class '{this.Name}'");
            ;
            var q = new GraphQLQuery()
            {
                Query = CreateAggregateQuery().Meta().ToString()
            };
            var res = await _connection.Schema.RawQuery(q);
            if (res == null) throw new Exception("Error when performing the count query");
            if (res.Errors == null)
            {
#pragma warning disable CS8602 // Disable warning for derefernce potentially null value since I know it should be ok.
                var count = res.Data["Aggregate"][this.Name][0]["meta"].Value<int>("count");
#pragma warning restore CS8602
                return count;
            }
            throw new Exception(res.Errors.ToString());
        }

        public async Task<int> CountObjectsByProperty<T>(string propertyName, T value)
        {
            if (!typeof(P).GetFields().Where(p => p.Name == propertyName).Any())
                throw new Exception($"'{propertyName}' is not a valid property for class '{this.Name}'");
            if (_connection == null) throw new Exception($"Empty connection while counting objects of class '{this.Name}'");
            var aq = CreateAggregateQuery();
            aq.Filter.Where(When<P,T>.Equal(propertyName, value));
            aq.Meta();
            var q = new GraphQLQuery()
            {
                Query = aq.ToString()
            };
            var res = await _connection.Schema.RawQuery(q);
            if (res == null) throw new Exception("Error when performing the count query");
            if (res.Errors == null)
            {
#pragma warning disable CS8602 // Disable warning for derefernce potentially null value since I know it should be ok.
                var count = res.Data["Aggregate"][this.Name][0]["meta"].Value<int>("count");
#pragma warning restore CS8602
                return count;
            }
            throw new Exception(res.Errors.ToString());
        }

        public async Task<bool> Validate(WeaviateObject<P> obj)
        {
            if (_connection == null) throw new Exception($"Empty connection while validating object '{obj.Id}'");
            try
            {
                await _connection.Client.Objects_validateAsync(obj);
                return true;
            } catch {
                return false;
            }
        }

        public async Task<bool> ExistsObject(Guid id,string consistency_level = "QUORUM", string? tenant=null)
        {
            if (_connection == null) throw new Exception($"Empty connection while checking object '{id}'");
            try
            {
                await _connection.Client.Objects_class_headAsync(this.Name, id, consistency_level, tenant);
                return true;
            } catch {
                return false;
            }
        }

        public async Task<NodesStatusResponse> CheckNodesStatus()
        {
            if (_connection == null) throw new Exception($"Empty connection while checking nodes status for class '{this.Name}'");
            return await _connection.Client.Nodes_get_classAsync(this.Name);
        }

        public Classification CreateClassification()
        {
            return new Classification() { Class = this.Name };
        }

        public async Task<Classification> StartClassification(Classification conf)
        {
            if (_connection == null) throw new Exception($"Empty connection while starting classification for class '{this.Name}'");
            return await _connection.Client.Classifications_postAsync(conf);
        }

        public async Task<Classification> ClassificationsStatus(string id)
        {
            if (_connection == null) throw new Exception($"Empty connection while requesting classification status '{this.Name}'");
            return await _connection.Client.Classifications_getAsync(id);
        }
    }
}
