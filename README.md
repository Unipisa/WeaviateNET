# Weaviate.NET
This library is designed to wrap [Weaviate](https://weaviate.io/) vector DB 
for .NET core. The core API has been wrapped with *NSwagStudio* and then some
extra layer has been added. The API mapping is incomplete and Unit tests
should be generalized (they currently use a demo instance).

It is designed to manipulate Weaviate objects and not just to adapt the
connection to some LLM framework as other NuGet packages do.

## What's new in version 1.21.1.2
- Now it is possible to define fields in classes that are not mapped to Weaviate
properties if annotated with JsonIgnore attribute.

## Implementation status
The library implements almost all the schema, class, and object manipulation.
It is possible to perform GraphQL queries though the current implementation
is pretty raw. Thanks to Newtonsoft JObject it is possible to parse returned
data pretty easily.

Note that it is still possible to use the NSwag generated API from Weaviate
swagger file by using the *Client* property of the *WeaviateDB* connection.
By missing APIs we intend that the calls have not yet been nicely wrapped
in the object model and tested.

### Missing APIs

- References management
- /batch/references endpoint is not implemented

### Untested APIs

- Tenant
- Shards
- Backups
- Classifications

## Roadmap
After implementing the missing endpoints in the object model we will work in
providing a better GraphQL experience. In the ideal world a LINQ provider would
be great to integrate queries into C#.

## Package versioning
The [Weaviate.NET](https://www.nuget.org/packages/WeaviateNET/) package on NuGet
follows the versioning schema of Weaviate: version 1.20.5.1 of the package is the
first release tested on version 1.20.5 of Weaviate. So look at the last number to
check the library revision.

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

If we want to expose the id field in the *Movie* class we can use the *JsonIgnore* attribute
to avoid mapping it to a property:

        [JsonIgnore]
        public Guid? id;

**Important:** If you use a class with less properties the library *will work*, this is
useful to perform schema updates or property masking. Magic is performed by Newtonsoft
serializer/deserializer.

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

## Previous changes

### version 1.21.1.1
Now the *DateTime?* type is supported. The library will convert the DateTime? to
*DatTime*.

### version 1.21.0.2
Added method *CountObjectsByProperty* to count object with a specific value in
a property.

### Version 1.21.0.1
Added the CountObjects method to WeaviateClass.

Tested against version 1.21.0 of Weaviate

### Version 1.20.5.3
Added the ListObjects method to WeaviateClass. Apparently if you omit the *limit*
parameter you get an empty list so the parameter is defaulted to 25.
