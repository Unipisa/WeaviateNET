namespace WeaviateNET
{
    public class WeaviateDB
    {
        private string _baseUrl;
        private HttpClient _httpClient;
        private WeaviateClient _weaviateClient;
        private Schema _schema;

        internal WeaviateClient Client
        {
            get { return _weaviateClient; }
        }

        public WeaviateDB(string baseUrl, string? token = null)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            _weaviateClient = new WeaviateClient(baseUrl, _httpClient);
            _schema = new Schema();
            _schema._connection = this;
        }

        public Task<Meta> GetMeta()
        {
           return _weaviateClient.Meta_getAsync();
        }

        public Schema Schema { get { return _schema; } }
    }
}