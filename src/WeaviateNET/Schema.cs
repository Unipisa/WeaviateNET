using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public partial class Schema
    {
        [JsonIgnore]
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

        public ICollection<FieldInfo> PersistentFields<P>()
        {
            var flds = typeof(P).GetFields();
            return flds.Where(f => !f.CustomAttributes.Where(a => a.AttributeType == typeof(JsonIgnoreAttribute)).Any()).ToList();
        }

        public async Task<WeaviateClass<P>> NewClass<P>(string name) where P : class, new()
        {
            if (_connection == null) throw new Exception($"Empty connection while creating class '{name}'");

            var c = new WeaviateClass<P>() { Name=name };

            var pclass = typeof(P);

            var vectorIndexConfig = pclass.GetCustomAttribute<VectorIndexConfigAttribute>();
            if (vectorIndexConfig != null) c.VectorIndexConfig = new Dictionary<string, object>(){ { "distance", vectorIndexConfig.Distance } };
            var replicationConfig = pclass.GetCustomAttribute<ReplicationConfigAttribute>();
            if (replicationConfig != null) c.ReplicationConfig.Factor = replicationConfig.Factor;
            var indexStopwords = pclass.GetCustomAttribute<IndexStopwordsAttribute>();
            var indexTimestamps = pclass.GetCustomAttribute<IndexTimestampsAttribute>();
            var indexNullState = pclass.GetCustomAttribute<IndexNullStateAttribute>();
            var indexPropertyLength = pclass.GetCustomAttribute<IndexPropertyLengthAttribute>();
            var bm25indexConfig = pclass.GetCustomAttribute<BM25IndexAttribute>();
            if (c.InvertedIndexConfig == null && 
                (indexStopwords != null || 
                indexTimestamps != null || 
                indexNullState != null || 
                indexPropertyLength != null || 
                bm25indexConfig != null))
                c.InvertedIndexConfig = new InvertedIndexConfig();
            if (indexStopwords != null)
            {
                if (c.InvertedIndexConfig.Stopwords == null)
                    c.InvertedIndexConfig.Stopwords = new StopwordConfig();
                c.InvertedIndexConfig.Stopwords.Preset = indexStopwords.Preset;
                c.InvertedIndexConfig.Stopwords.Additions = indexStopwords.Additions;
                c.InvertedIndexConfig.Stopwords.Removals = indexStopwords.Removals;
            }
            if (indexTimestamps != null) c.InvertedIndexConfig.IndexTimestamps = indexTimestamps.Enabled;
            if (indexNullState != null) c.InvertedIndexConfig.IndexNullState = indexNullState.Enabled;
            if (indexPropertyLength != null) c.InvertedIndexConfig.IndexPropertyLength = indexPropertyLength.Enabled;
            if (bm25indexConfig != null)
            {
                if (c.InvertedIndexConfig.Bm25 == null)
                    c.InvertedIndexConfig.Bm25 = new BM25Config();
                c.InvertedIndexConfig.Bm25.K1 = bm25indexConfig.K1;
                c.InvertedIndexConfig.Bm25.B = bm25indexConfig.B;
            }
            var multiTenancyConfig = pclass.GetCustomAttribute<MultiTenancyAttribute>();
            if (multiTenancyConfig != null) c.MultiTenancyConfig.Enabled = multiTenancyConfig.Enabled;

            var flds = PersistentFields<P>();
            c.Properties = new List<Property>(flds.Count);

            foreach (var f in flds)
            {
                var p = Property.Create(f.FieldType, f.Name);
                var tokenizationModeConfig = f.GetCustomAttribute<TokenizationAttribute>();
                if (tokenizationModeConfig != null) p.Tokenization = tokenizationModeConfig.Mode;
                var indexFilterableConfig = f.GetCustomAttribute<IndexFilterableAttribute>();
                if (indexFilterableConfig != null) p.IndexFilterable = indexFilterableConfig.Enabled;
                var indexSearchableConfig = f.GetCustomAttribute<IndexSearchableAttribute>();
                if (indexSearchableConfig != null) p.IndexSearchable = indexSearchableConfig.Enabled;
                // FIXME: not supporting references yet
                c.Properties.Add(p);
            }
            var nc = await _connection.Client.Schema_objects_createAsync(c);
            

            if (nc == null) throw new Exception($"Error while creating the class '{c.Name}'");
            var ret = new WeaviateClass<P>(nc);
            ret._connection = _connection;
            return ret;
        }

        public WeaviateClass<P>? GetClass<P>(string name) where P : class, new()
        {
            var c = this.Classes.FirstOrDefault(c => c.Name == name);
            if (c == null) return null;
            var ret = new WeaviateClass<P>(c);
            ret._connection = _connection; // Connection is *not* copied
            return ret;
        }

        public async Task<SchemaClusterStatus> ClusterStatus()
        {
            if (_connection == null) throw new Exception($"Empty connection while retrieving cluster status");
            return await _connection.Client.Schema_cluster_statusAsync();
        }

        public async Task<GraphQLResponse> RawQuery(GraphQLQuery query)
        {
            if (_connection == null) throw new Exception($"Empty connection while querying schema '{query}'");
            return await _connection.Client.Graphql_postAsync(query);
        }

        public async Task<ICollection<GraphQLResponse>> RawQueryBatch(ICollection<GraphQLQuery> queries)
        {
            if (_connection == null) throw new Exception($"Empty connection while querying schema '{queries}'");
            return await _connection.Client.Graphql_batchAsync(queries);
        }

        public async Task<BackupCreateResponse> BackupStart(string backend, BackupCreateRequest request)
        {
            if (_connection == null) throw new Exception($"Empty connection while starting backup on '{backend}'");
            return await _connection.Client.Backups_createAsync(backend, request);
        }

        public async Task<BackupCreateStatusResponse> BackupStatus(string backend, string id)
        {
            if (_connection == null) throw new Exception($"Empty connection while checking backup '{id}'");
            return await _connection.Client.Backups_create_statusAsync(backend, id);
        }

        public async Task<BackupRestoreResponse> RestoreBackup(string backend, string id, BackupRestoreRequest request)
        {
            if (_connection == null) throw new Exception($"Empty connection while restoring backup '{id}'");
            return await _connection.Client.Backups_restoreAsync(backend, id, request);
        }

        public async Task<BackupRestoreStatusResponse> RestoreBackupStatus(string backend, string id)
        {
            if (_connection == null) throw new Exception($"Empty connection while checking backup restore '{id}'");
            return await _connection.Client.Backups_restore_statusAsync(backend, id);
        }

        public async Task<NodesStatusResponse> CheckNodesStatus()
        {
            if (_connection == null) throw new Exception($"Empty connection while checking nodes status");
            return await _connection.Client.Nodes_getAsync();
        }
    }
}
