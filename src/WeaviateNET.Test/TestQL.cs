using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaviateNET.Query.AdditionalProperty;
using WeaviateNET.Query.ConditionalOperator;

namespace WeaviateNET.Test
{
    [TestClass]
    public class TestQL
    {
        WeaviateDB? weaviateDB;
        WeaviateClass<Movie>? movieClass;
        IConfigurationRoot? Configuration;

        [TestInitialize]
        public void Init()
        {
            IConfigurationBuilder config = new ConfigurationBuilder()
            .AddUserSecrets<TestSchema>();
            Configuration = config.Build();

            weaviateDB = new WeaviateDB(Configuration["Weaviate:ServiceEndpoint"], Configuration["Weaviate:ApiKey"]);
            weaviateDB.Schema.Update().Wait();
            movieClass = weaviateDB.Schema.GetClass<Movie>(Movie.ClassName);
        }

        [TestMethod]
        public async Task TestSimpleGet()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(movieClass);
            var q = movieClass.CreateGetQuery(false);
            q.Fields.AddFields("film", "genre");
            var query = new GraphQLQuery()
            {
                Query = q.ToString()
            };
            var ret = await weaviateDB.Schema.RawQuery(query);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
        }

        [TestMethod]
        public async Task TestGetSearchOps1()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(movieClass);
            var q = movieClass.CreateGetQuery(false);
            q.Fields.AddFields("film", "genre");
            q.Filter
                .NearText("robot")
                .Limit(2);
            var query = new GraphQLQuery()
            {
                Query = q.ToString()
            };
            var ret = await weaviateDB.Schema.RawQuery(query);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
        }

        [TestMethod]
        public async Task TestGetSearchOps2()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(movieClass);
            var q = movieClass.CreateGetQuery(false);
            q.Fields.AddFields("film", "genre");
            q.Filter
                .NearText(concept: "robot")
                .Limit(2)
                .Where(Conditional<Movie>.And(
                    When<Movie, string>.Equal("film", "WALL-E")
                    ));
            var query = new GraphQLQuery()
            {
                Query = q.ToString()
            };
            var ret = await weaviateDB.Schema.RawQuery(query);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
        }

        [TestMethod]
        public async Task TestGetSearchOps3()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(movieClass);
            var q = movieClass.CreateGetQuery(false);
            q.Fields.AddFields("film", "genre");
            q.Fields.Additional.Add(
                Additional.Id, 
                Additional.Distance);
            q.Filter
                .NearText("robot")
                .Limit(2)
                .Where(Conditional<Movie>.And(
                    When<Movie, string>.Equal("film", "WALL-E")
                    ));
            var query = new GraphQLQuery()
            {
                Query = q.ToString()
            };
            var ret = await weaviateDB.Schema.RawQuery(query);
            Assert.IsNotNull(ret);
            Assert.IsNull(ret.Errors);
        }

        [TestMethod]
        public async Task TestAggregateSearchOps1()
        {
            Assert.IsNotNull(weaviateDB);
            Assert.IsNotNull(movieClass);
            var q = movieClass.CreateAggregateQuery();

            Assert.ThrowsException<Exception>(() => q.Filter
                .NearText("robot")
                .Limit(2));

            q.Filter.Clear();
            q.Filter
                .NearText("robot")
                .Tenant("mio");

            var query = new GraphQLQuery()
            {
                Query = q.ToString()
            };
            //var ret = await weaviateDB.Schema.RawQuery(query);
            //Assert.IsNotNull(ret);
            //Assert.IsNull(ret.Errors);
        }

    }
}
