using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections;
using System.Text.RegularExpressions;
using WeaviateNET;

namespace WeaviateNET.Test
{
    /// <summary>
    /// Example of property definition. Notice that you *should* use only
    /// public fields with low case names (otherwise serialization/deserialization may fail).
    /// Weaviate corrects property names into lower case.
    /// </summary>
    [VectorIndexConfig(Distance = DistanceMetric.Dot), IndexNullState]
    public class Document
    {
        [JsonIgnore]
        public Guid? id;

        public int intData;
        public string? textData;
        public DateTime dateData;
        public DateTime? optionalDate;
        [Tokenization(PropertyTokenization.Whitespace)]
        public string[]? textArrayData;
    }

    [TestClass]
    public class TestSchema
    {
        WeaviateDB? weaviateDB;
        IConfigurationRoot? Configuration;

        [TestInitialize]
        public void Init()
        {
            IConfigurationBuilder config = new ConfigurationBuilder()
            .AddUserSecrets<TestSchema>();
            Configuration = config.Build();

            weaviateDB = new WeaviateDB(Configuration["Weaviate:ServiceEndpoint"], Configuration["Weaviate:ApiKey"]);
        }

        [TestCleanup]
        public async Task CleanupClasses()
        {
            Assert.IsNotNull(weaviateDB);
            await weaviateDB.Schema.Update();
            foreach (var c in weaviateDB.Schema.Classes)
            {
                if (Regex.IsMatch(c.Name, "^Test\\d+$"))
                {
                    await c.Delete();
                }
            }
            await weaviateDB.Schema.Update();
        }

        [TestMethod]
        public async Task TestMeta()
        {
            Assert.IsNotNull(weaviateDB);
            var s = await weaviateDB.GetMeta();
            Assert.IsTrue(s.Version.StartsWith("1."));
        }

        [TestMethod]
        public async Task TestLoadSchema()
        {
            Assert.IsNotNull(weaviateDB);
            await weaviateDB.Schema.Update();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestCreateClass()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            await weaviateDB.Schema.Update();
            var n = weaviateDB.Schema.Classes.Count();
            var c = await weaviateDB.Schema.NewClass<NoProperties>(name);
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);
            await c.Delete();
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        }

        [TestMethod]
        public async Task TestCreateClassWithFields()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            await weaviateDB.Schema.Update();
            var n = weaviateDB.Schema.Classes.Count();
            var c = await weaviateDB.Schema.NewClass<Document>(name);
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);
            Assert.AreEqual(c.Properties.Count, Schema.PersistentFields<Document>().Count);
            await c.Delete();
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        }

        // This test fails with generated code because cleanupIntervalSeconds is reported as
        // double? but it is expected to be int? (I changed the generated output)
        [TestMethod]
        public async Task TestUpdateClass()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            var desc = $"Description for {name}";
            await weaviateDB.Schema.Update();
            var n = weaviateDB.Schema.Classes.Count();
            var c = await weaviateDB.Schema.NewClass<NoProperties>(name);
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);
            c.Description = desc;
            await c.Save();
            c.Description = "";
            await c.Update();
            Assert.AreEqual(desc, c.Description);
            await c.Delete();
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        }

        [TestMethod]
        public async Task TestClusterStatus()
        {
            Assert.IsNotNull(weaviateDB);
            var s = await weaviateDB.Schema.ClusterStatus();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.NodeCount > 0);
        }

        [TestMethod]
        public async Task TestAddProperty()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            await weaviateDB.Schema.Update();
            var n = weaviateDB.Schema.Classes.Count();
            var c = await weaviateDB.Schema.NewClass<NoProperties>(name);
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);

            await c.AddProperty(Property.Create<string>("PropertyName"));

            Assert.IsTrue(c.Properties.Count == 1);

            await c.Delete();
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        }

        [TestMethod]
        public async Task TestAddObject()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            await weaviateDB.Schema.Update();
            var n = weaviateDB.Schema.Classes.Count();
            var c = await weaviateDB.Schema.NewClass<Document>(name);
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);
            Assert.AreEqual(c.Properties.Count, Schema.PersistentFields<Document>().Count);

            await c.Delete();
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        }

        //This test covers only reading shard status, a test for setting the status needed in future
        [TestMethod]
        public async Task TestShards()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            await weaviateDB.Schema.Update();
            var n = weaviateDB.Schema.Classes.Count();
            var c = await weaviateDB.Schema.NewClass<NoProperties>(name);
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);

            var ret = await c.GetShardsStatus();
            Assert.IsTrue(ret.Count > 0);
            await c.Delete();
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        }

        [TestMethod]
        public async Task TestNodeStatus()
        {
            Assert.IsNotNull(weaviateDB);
            var r = await weaviateDB.Schema.CheckNodesStatus();
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Nodes.Count > 0);
        }

        //This test require better understanding of multitenancy model
        //[TestMethod]
        //public async Task TestTenants()
        //{
        //    Assert.IsNotNull(weaviateDB);
        //    var name = $"Test{DateTime.Now.Ticks}";
        //    await weaviateDB.Schema.Update();
        //    var n = weaviateDB.Schema.Classes.Count();
        //    var c = await weaviateDB.Schema.NewClass(new Class { Name = name, MultiTenancyConfig=new MultiTenancyConfig() { Enabled=true } }) ;
        //    await weaviateDB.Schema.Update();
        //    Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
        //    Assert.AreEqual(c.Name, name);

        //    await c.AddTenants(new List<Tenant>(new Tenant[] { new Tenant() { Name = "Tenant A" }, new Tenant() { Name = "Tenant B" } }));

        //    var ret = await c.GetTenants();

        //    await c.Delete();
        //    await weaviateDB.Schema.Update();
        //    Assert.AreEqual(n, weaviateDB.Schema.Classes.Count());
        //}
    }
}