using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public partial class WeaviateObject<P> where P : class, new()
    {
        [JsonIgnore]
        internal WeaviateClassBase? classType;

        internal WeaviateObject() {
#if AUTO_GENERATE_OID
            // Better have an id for each object
            // You can still nullate the field, however the AUTO_GENERATE_OID compiler directive enables this behaviour
            this.Id = Guid.NewGuid();
#endif
            this.Properties = new P();
        }

        internal void CopyFrom(WeaviateObject<P> src)
        {
            this.Additional = src.Additional;
            this.Class = src.Class;
            this.CreationTimeUnix = src.CreationTimeUnix;
            this.Id = src.Id;
            this.LastUpdateTimeUnix = src.LastUpdateTimeUnix;
            this.Properties = src.Properties;
            this.Tenant = src.Tenant;
            this.Vector = src.Vector;
            this.VectorWeights = src.VectorWeights;
        }

        internal WeaviateObject(WeaviateClassBase classType, Guid id=new Guid()) : base()
        {
            this.classType = classType;
            this.Class = classType.Name;
            if (id != Guid.Empty)
            {
                this.Id = id;
            }
        }

        public async Task Update(string consistency_level = "QUORUM", string? include = null, string? node_name = null, string? tenant = null)
        {
            if (!this.Id.HasValue) throw new Exception($"Attempt to delete an object without Id");
            if (classType == null || classType._connection == null) throw new Exception($"Invalid connection on object '{this.Id}'");
            var o = await classType._connection.Client.Objects_class_getAsync<P>(classType.Name, this.Id.Value, include, consistency_level, node_name, tenant);
            CopyFrom(o);
        }

        public async Task Save(bool usePatch=false, string consistency_level = "QUORUM", string? tenant = null)
        {
            if (!this.Id.HasValue) throw new Exception($"Attempt to delete an object without Id");
            if (classType == null || classType._connection == null) throw new Exception($"Invalid connection on object '{this.Id}'");
            if (!usePatch)
            {
                await classType._connection.Client.Objects_class_putAsync(classType.Name, this.Id.Value, this, consistency_level);
            } else
            {
                await classType._connection.Client.Objects_class_patchAsync(classType.Name, this.Id.Value, this, consistency_level);
            }
        }

        public async Task Delete(string consistency_level="QUORUM", string? tenant=null)
        {
            if (!this.Id.HasValue) throw new Exception($"Attempt to delete an object without Id");
            if (classType == null || classType._connection == null) throw new Exception($"Invalid connection on object '{this.Id}'");
            await classType._connection.Client.Objects_class_deleteAsync(classType.Name, this.Id.Value, consistency_level, tenant);
        }
    }
}
