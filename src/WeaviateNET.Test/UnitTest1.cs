using WeaviateNET;

namespace WeaviateNET.Test
{
    [TestClass]
    public class UnitTest1
    {
        WeaviateDB? weaviateDB;

        [TestInitialize]
        public void Init()
        {
            weaviateDB = new WeaviateDB("http://131.114.72.55:8080/v1", "f69c2ec8-991c-44b9-b038-cd39d62f0ea2");
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
            var c = await weaviateDB.Schema.NewClass(new Class { Name = name });
            await weaviateDB.Schema.Update();
            Assert.AreEqual(n + 1, weaviateDB.Schema.Classes.Count());
            Assert.AreEqual(c.Name, name);
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
            var c = await weaviateDB.Schema.NewClass(new Class { Name = name });
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
    }
}