# Weaviate.NET
This library is designed to wrap [Weaviate](https://weaviate.io/) vector DB 
for .NET core. The core API has been wrapped with *NSwagStudio* and then some
extra layer has been added. The API mapping is incomplete and Unit tests
should be generalized (they currently use a demo instance).

It is designed to manipulate Weaviate objects and not just to adapt the
connection to some LLM framework as other NuGet packages do.

## Implementation status
The library implements almost all the schema, class, and object manipulation.
It is possible to perform GraphQL queries though the current implementation
is pretty raw. Thanks to Newtonsoft JObject it is possible to parse returned
data pretty easily.

Missing implementation areas (notice that you can still use the NSwag generated 
wrapper calls):

- Proper references management
- /backups/* endpoints are not implemented
- /batch/references endpoint is not implemented
- /classifications/* endpoints are not implemented
- /nodes/* endpoints are not implemented
- Tenants

## Roadmap
After implementing the missing endpoints in the object model we will work in
providing a better GraphQL experience. In the ideal world a LINQ provider would
be great to integrate queries into C#.

## Using the library

The data model is respectful of the database object model. To start create a
connection (using the authorization key) and update the Schema to load the
configuration:

	weaviateDB = new WeaviateDB("https://yourhost/v1", "WEAVIATE_KEY");
	await weaviateDB.Update();

In the *Weaviate* model objects are made of a set of fields (i.e. properties)
with the appropriate data type (see *WeaviateDataType* class for the full list).
*Weaviate.NET* maps properties to class fields (with few restrictions of course).
So we can define the class *Movie* to describe a movie document:

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

Notice that all names follow the camel notation, *if you use pascal notation Weaviate 
will lower case the first letter*.

We can create the class in the schema and load data (of type *WeaviateObject< Movie>*):

    var mc = weaviateDB.Schema.NewClass<Movie>("MovieDBTest");
    Movie[] movies = ReadMoviesDBFromCsv();
    var toadd = new List<WeaviateObject<Movie>>();
    foreach (var m in movies) {
      var d = mc.Create(); // create the document
      d.Properties = m;    // set the properties (of type Movie)
      toadd.Add(d);
    }
    await mc.Add(toadd);

To query the database you use explicitly the GraphQL syntax and the ability of JObject
to deserialize a field. This basic approach will be supported in the future but 
a better abstraction will be provided.

            var q = new GraphQLQuery();
            q.Query = @"{
      Get {
        MovieDBTest(
          limit: 5
          nearText: { concepts: [ "robot in the future" ] }
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
            var d = ret.Data["Get"];
            var a = data.ToObject<Movie[]>();
