using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<WeaviateClass<P>> NewClass<P>(string name) where P : class, new()
        {
            var c = new WeaviateClass<P>() { Name=name };

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
