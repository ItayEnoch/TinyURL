using MongoDB.Driver;
using TinyURL.Models;

namespace TinyURL.Repositories
{
    public interface IUrlRepository
    {
        Task<List<ShortUrlCls>> Get();
        Task<ShortUrlCls> Get(string id);
        Task Create(ShortUrlCls newUrl);
    }

    public class UrlRepository : IUrlRepository
    {
        private readonly IMongoDatabase _database;
        private readonly string _urlCollectionName;

        public UrlRepository(Settings settings)
        {
            var dbClient = new MongoClient(settings.MongoDbConnectionString);
            _database = dbClient.GetDatabase(settings.DataBaseName);
            _urlCollectionName = settings.UrlCollectionName;
        }

        public async Task<List<ShortUrlCls>> Get()
        {
            var urlCollection = _database.GetCollection<ShortUrlCls>(_urlCollectionName);
            var urlCollectionList = await urlCollection.Find(_ => true).ToListAsync();

            return urlCollectionList;
        }

        public async Task<ShortUrlCls> Get(string id)
        {
            var urlCollection = _database.GetCollection<ShortUrlCls>(_urlCollectionName);
            var urlCollectionList = await urlCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return urlCollectionList;
        }

        public Task Create(ShortUrlCls newUrl)
        {
            var urlCollection = _database.GetCollection<ShortUrlCls>(_urlCollectionName);

            urlCollection.InsertOneAsync(newUrl);

            return Task.CompletedTask;
        }
    }
}
