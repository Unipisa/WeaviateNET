using System.Collections;
using System.Text.RegularExpressions;
using WeaviateNET;

namespace WeaviateNET.Test
{
    [TestClass]
    public class TestContent
    {
        WeaviateDB? weaviateDB;
        WeaviateClass<Document>? wclass;
        // Ensure the same value in object creations
        DateTime dataValue;
        Random rnd = new Random();

        private async Task<WeaviateClass<P>> CreateClass<P>() where P : class, new()
        {
            Assert.IsNotNull(weaviateDB);
            var name = $"Test{DateTime.Now.Ticks}";
            await weaviateDB.Schema.Update();
            var c = await weaviateDB.Schema.NewClass<P>(name);
            await weaviateDB.Schema.Update();
            return c;
        }

        [TestInitialize]
        public void Init()
        {
            weaviateDB = new WeaviateDB("http://131.114.72.55:8080/v1", "f69c2ec8-991c-44b9-b038-cd39d62f0ea2");
            var j = CreateClass<Document>();
            j.Wait();
            wclass = j.Result;
            dataValue = DateTime.Now;
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
        public async Task TestAddObject()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(wclass);

            WeaviateObject<Document> data = prepareObject();

            await wclass.Add(data);

            Assert.IsNotNull(data.Id);
            Guid id = (Guid)data.Id;

            var o = await wclass.Get(id);
            assertPropertiesAreEqual(data, o);

            Assert.IsTrue(await wclass.ExistsObject(data.Id.Value));
            Assert.IsFalse(await wclass.ExistsObject(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task TestAddObjects()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(wclass);

            var objs = new List<WeaviateObject<Document>>();
            for (var i = 0; i < 128; i++)
            {
                objs.Add(prepareRandomObject());
            }

            var ret = await wclass.Add(objs);

            Assert.AreEqual(objs.Count, ret.Count);

            for (var i = 0; i < 10; i++)
            {
                var o = objs[rnd.Next(128)];
                Assert.IsNotNull(o);
                Assert.IsNotNull(o.Id);
                var d = await wclass.Get((Guid)o.Id);
                assertPropertiesAreEqual(o, d);
            }
        }

        [TestMethod]
        public async Task TestUpdateObject()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(wclass);

            var data = prepareObject();
            Assert.IsNotNull(data.Id);

            await wclass.Add(data);
            
            await data.Update();

            var check = prepareObject();
            assertPropertiesAreEqual(data, check);
        }

        [TestMethod]
        public async Task TestSaveObject()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(wclass);

            var data = prepareObject();
            var check = prepareObject();

            Assert.IsNotNull(data.Id);

            await wclass.Add(data);

            Assert.IsNotNull(data.Properties.textArrayData);
            Assert.IsNotNull(check.Properties.textArrayData);

            data.Properties.textArrayData[0] = "This";
            check.Properties.textArrayData[0] = data.Properties.textArrayData[0];
            data.Properties.intData = 84;
            check.Properties.intData = data.Properties.intData;

            await data.Save();

            await data.Update();

            assertPropertiesAreEqual(data, check);

            data.Properties.textArrayData[0] = "THIS";
            check.Properties.textArrayData[0] = data.Properties.textArrayData[0];
            data.Properties.intData = 168;
            check.Properties.intData = data.Properties.intData;

            await data.Save(usePatch: true);

            await data.Update();

            assertPropertiesAreEqual(data, check);
        }

        [TestMethod]
        public async Task TestCheckAndDeleteObject()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(wclass);

            var data = prepareObject();

            Assert.IsNotNull(data.Id);

            Assert.IsFalse(await wclass.ExistsObject(data.Id.Value));

            await wclass.Add(data);

            Assert.IsTrue(await wclass.ExistsObject(data.Id.Value));

            await data.Delete();

            Assert.IsFalse(await wclass.ExistsObject(data.Id.Value));
        }


        /// <summary>
        /// The Object model of Weaviate.NET is basically such that validation is correct by default
        /// However this test is helpful during library updates.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestValidate()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(wclass);

            var data = prepareObject();

            Assert.IsNotNull(data.Id);

            Assert.IsTrue(await wclass.Validate(data));
        }


        private void assertPropertiesAreEqual(WeaviateObject<Document> data, WeaviateObject<Document> o)
        {
            Assert.AreEqual(data.Properties.textData, o.Properties.textData);
            Assert.AreEqual(data.Properties.dateData, o.Properties.dateData);
            Assert.AreEqual(data.Properties.intData, o.Properties.intData);
            Assert.IsNotNull(o.Properties.textArrayData);
            Assert.IsNotNull(data.Properties.textArrayData);
            Assert.AreEqual(data.Properties.textArrayData.Length, o.Properties.textArrayData.Length);
            for (var i = 0; i < data.Properties.textArrayData.Length; i++)
            {
                Assert.AreEqual(data.Properties.textArrayData[i], o.Properties.textArrayData[i]);
            }
        }

        private WeaviateObject<Document> prepareObject()
        {
            Assert.IsNotNull(wclass);
            var data = wclass.Create();
            data.Properties.textData = "this is text Data";
            data.Properties.dateData = dataValue;
            data.Properties.intData = 42;
            data.Properties.textArrayData = new string[] { "this", "is", "a", "test" };
            return data;
        }

        private WeaviateObject<Document> prepareRandomObject()
        {
            var val = rnd.Next(1024);
            Assert.IsNotNull(wclass);
            var data = wclass.Create();
            data.Properties.textData = $"this is text Data #{val}";
            data.Properties.dateData = dataValue + TimeSpan.FromSeconds(val);
            data.Properties.intData = val;
            data.Properties.textArrayData = new string[] { "this", "is", "a", "test", $"#{val}" };
            return data;
        }
    }
}