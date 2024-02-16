using MongoDB.Driver;
using TinyURL.Models;
using TinyURL.Repositories;

namespace TinyURL.Services
{
    public class HandleUrlService
    {
        private readonly IUrlRepository _mongoRepository;
        private readonly string _baseUrl;

        public HandleUrlService(Settings settings, IUrlRepository urlRepository)
        {
            _mongoRepository = urlRepository;
            _baseUrl = settings.BaseUrl;
        }

        public async Task<List<ShortUrlCls>> GetAsync()
        {
            var collection = await _mongoRepository.Get();

            return collection;
        }

        public async Task<ShortUrlCls?> GetUrlClsAsync(string shortUrl)
        {
            var collection = await GetAsync();

            var url = collection.Where(x => x.ShortUrl == shortUrl).FirstOrDefault();
            if (url != null)
            {
                return url;
            }
            else
            {
                Console.WriteLine("Url not found");
                return null;
            }
        }

        public async Task<string> GetFullUrlAsync(string shortUrl)
        {
            var collection = await GetAsync();

            var url = collection.Where(x => x.ShortUrl == shortUrl).FirstOrDefault();
            if (url != null)
            {
                return url.OriginalUrl;
            }
            else
            {
                Console.WriteLine("Url not found");
                return string.Empty;
            }
        }

        public async Task<ShortUrlCls> CreateShortUrlAsync(string url)
        {
            var collection = await GetAsync();
            
            //make url short
            var hashUrl = string.Empty;
            hashUrl = CreateMD5Hash(url);
            var shortUrl = Path.Combine(_baseUrl, hashUrl);

            //insert if not already in collection
            var urlExists = collection.Where(x => x.ShortUrl == shortUrl).Any();
            if (urlExists)
            {
                Console.Write("Url already in db");
            }
            else
            {
                var newUrl = new ShortUrlCls()
                {
                    OriginalUrl = url,
                    ShortUrl = shortUrl,
                    CreateDate = DateTime.UtcNow
                };

                await _mongoRepository.Create(newUrl);

                return newUrl;
            }

            return new ShortUrlCls();
        }

        private static string CreateMD5Hash(string fullUrl)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(fullUrl);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
