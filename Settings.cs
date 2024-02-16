using MongoDB.Driver;
using TinyURL.Models;

namespace TinyURL
{
    public class Settings
    {
        public string MongoDbConnectionString { get; set; }
        public string DataBaseName { get; set; }
        public string UrlCollectionName { get; set; }
        public string BaseUrl { get; set; }

        public Settings()
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();

            var config = appSettings.GetSection("appConfiguration").Get<AppConfiguration>();

            MongoDbConnectionString = config.ConnectionString;
            DataBaseName = config.DatabaseName;
            UrlCollectionName = config.UrlCollectionName;
            BaseUrl = config.BaseUrl;
        }
    }
}
