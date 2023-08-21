using Newtonsoft.Json;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using WeaviateNET;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace WeaviateNET.Test
{
    public class Movie
    {
        public string? film;
        public string? genre;
        public string? leadStudio;
        public int audienceScore;
        public double profitability;
        public int rottenTomatoes;
        public double worldWideGross;
        public int year;
    }


    public class MovieReduced
    {
        public string? film;
        public string? genre;
        public int year;
    }

    public class MovieCsvRecord
    {
        [Name("Film")]
        public string? film { get; set; }
        [Name("Genre")]
        public string? genre { get; set; }
        [Name("Lead Studio")]
        public string? leadStudio { get; set; }
        [Name("Audience score %")]
        public int audienceScore { get; set; }
        [Name("Profitability")]
        public double profitability { get; set; }
        [Name("Rotten Tomatoes %")]
        public int rottenTomatoes { get; set; }
        [Name("Worldwide Gross")]
        public double worldWideGross { get; set; }
        [Name("Year")]
        public int year { get; set; }
    }


    [TestClass]
    public class TestContent
    {
        WeaviateDB? weaviateDB;
        IConfigurationRoot? Configuration;
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

        private async Task PopulateMovieDB()
        {
            Assert.IsNotNull(weaviateDB);
            await weaviateDB.Schema.Update();
            var c = weaviateDB.Schema.GetClass<Movie>("MovieDBTest");

            // To clean the DB usually is commented
            //if (c != null)
            //{
            //    await c.Delete();
            //    c = null;
            //}

            if (c == null)
            {
                c = await weaviateDB.Schema.NewClass<Movie>("MovieDBTest");

                var dlllocation = new System.IO.FileInfo(GetType().Assembly.Location);
                Assert.IsNotNull(dlllocation.Directory);
                var dbfile = System.IO.Path.Combine(dlllocation.Directory.FullName, "moviesdb.csv");
                using (var reader = new System.IO.StreamReader(dbfile))
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<MovieCsvRecord>();
                    var toadd = new List<WeaviateObject<Movie>>();
                    var num = 0;
                    foreach (var r in records)
                    {
                        var d = c.Create();
                        d.Properties.film = r.film;
                        d.Properties.genre = r.genre;
                        d.Properties.leadStudio = r.leadStudio;
                        d.Properties.audienceScore = r.audienceScore;
                        d.Properties.profitability = r.profitability;
                        d.Properties.rottenTomatoes = r.rottenTomatoes;
                        d.Properties.worldWideGross = r.worldWideGross;
                        d.Properties.year = r.year;
                        toadd.Add(d);
                        num++;
                    }
                    var ret = await c.Add(toadd);
                    Assert.AreEqual(num, ret.Count);
                }
            }
        }

        [TestInitialize]
        public void Init()
        {

            IConfigurationBuilder config = new ConfigurationBuilder()
            .AddUserSecrets<TestSchema>();
            Configuration = config.Build();

            weaviateDB = new WeaviateDB(Configuration["Weaviate:ServiceEndpoint"], Configuration["Weaviate:ApiKey"]);

            var j = CreateClass<Document>();
            j.Wait();
            wclass = j.Result;
            dataValue = DateTime.Now;

            var dbj = PopulateMovieDB();
            dbj.Wait();
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

        [TestMethod]
        public async Task TestGraphQLRaw()
        {
            Assert.IsNotNull(weaviateDB);
            var q = new GraphQLQuery();
            q.Query = @"{
  Get {
    MovieDBTest(
      limit: 5
    )
    {
      film
      genre
      leadStudio
      audienceScore
      profitability
      rottenTomatoes
      worldWideGross
      year
    }
  }
}";
            var ret = await weaviateDB.Schema.RawQuery(q);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
            var d = ret.Data["Get"];
            Assert.IsNotNull(d);
            var data = d["MovieDBTest"];
            Assert.IsNotNull(data);
            var a = data.ToObject<Movie[]>();
            Assert.IsNotNull(a);
            Assert.AreEqual(a.Length, 5);
        }

        [TestMethod]
        public async Task TestCheckReducedObject()
        {
            Assert.IsNotNull(weaviateDB);
            await weaviateDB.Schema.Update();
            var c = weaviateDB.Schema.GetClass<MovieReduced>("MovieDBTest");
            Assert.IsNotNull(c);
            var o = c.Create();
            o.Properties.film = "My test film";
            o.Properties.film = "SciFi";
            o.Properties.year = 2023;
            await c.Add(o);
            await o.Update();

            var q = new GraphQLQuery();
            q.Query = @"{
  Get {
    MovieDBTest(
      limit: 5
    )
    {
      film
      genre
      leadStudio
      audienceScore
      profitability
      rottenTomatoes
      worldWideGross
      year
    }
  }
}";
            var ret = await weaviateDB.Schema.RawQuery(q);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
            var d = ret.Data["Get"];
            Assert.IsNotNull(d);
            var data = d["MovieDBTest"];
            Assert.IsNotNull(data);
            var a = data.ToObject<MovieReduced[]>();
            Assert.IsNotNull(a);
            Assert.AreEqual(5, a.Length);

            q = new GraphQLQuery();
            q.Query = @"{
  Get {
    MovieDBTest
    {
      film
      genre
      leadStudio
      audienceScore
      profitability
      rottenTomatoes
      worldWideGross
      year
    }
  }
}";
            ret = await weaviateDB.Schema.RawQuery(q);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
            d = ret.Data["Get"];
            Assert.IsNotNull(d);
            data = d["MovieDBTest"];
            Assert.IsNotNull(data);
            a = data.ToObject<MovieReduced[]>();
            Assert.IsNotNull(a);
            Assert.AreEqual(78, a.Length);

            await o.Delete();

            ret = await weaviateDB.Schema.RawQuery(q);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
            d = ret.Data["Get"];
            Assert.IsNotNull(d);
            data = d["MovieDBTest"];
            Assert.IsNotNull(data);
            a = data.ToObject<MovieReduced[]>();
            Assert.IsNotNull(a);
            Assert.AreEqual(77, a.Length);
        }

        [TestMethod]
        public async Task TestListObjects()
        {
            Assert.IsNotNull(weaviateDB);
            await weaviateDB.Schema.Update();
            var moviedb = weaviateDB.Schema.GetClass<Movie>("MovieDBTest");
            Assert.IsNotNull(moviedb);
            var l = await moviedb.ListObjects(limit: 5);
            Assert.AreEqual(5, l.TotalResults);
            l = await moviedb.ListObjects();
            Assert.AreEqual(25, l.TotalResults);
            l = await moviedb.ListObjects(100);
            Assert.AreEqual(77, l.TotalResults);
        }

        [TestMethod]
        public async Task TestCountObjects()
        {
            Assert.IsNotNull(weaviateDB);
            await weaviateDB.Schema.Update();
            var moviedb = weaviateDB.Schema.GetClass<Movie>("MovieDBTest");
            Assert.IsNotNull(moviedb);
            Assert.AreEqual(77, await moviedb.CountObjects());
        }

        [TestMethod]
        public async Task TestClassNodeStatus()
        {
            Assert.IsNotNull(wclass);
            var r = await wclass.CheckNodesStatus();
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Nodes.Count > 0);
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