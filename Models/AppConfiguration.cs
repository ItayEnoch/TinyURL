namespace TinyURL.Models
{
    public class AppConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UrlCollectionName { get; set; }
        public string BaseUrl { get; set; }
    }
}
