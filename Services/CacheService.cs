using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using TinyURL.Models;

namespace TinyURL.Services
{
    public interface ICacheService
    {
        void AddItemToCache(ShortUrlClsExtended shortUrlCls);
        ShortUrlClsExtended? GetCachedItem(string url);
        void RemoveLastUsedFromCache();
        int GetCachedItemCount();
    }


    public class CacheService : ICacheService
    {
        private readonly List<ShortUrlClsExtended> _cache;
        private object _cacheLock = new object();
        private readonly int _maxCacheItems = 1000;

        public CacheService()
        {
            _cache = new List<ShortUrlClsExtended>();
            _cacheLock = new object();
        }

        public ShortUrlClsExtended? GetCachedItem(string url)
        {
            ShortUrlClsExtended urlItem = null;
            lock (_cacheLock)
            {
                urlItem = _cache.FirstOrDefault(x => x.ShortUrl == url);
            }

            return urlItem;
        }

        public int GetCachedItemCount()
        {
            int count = 0;
            lock (_cacheLock)
            {
                count = _cache.Count;
            }

            return count;
        }

        public void RemoveLastUsedFromCache()
        {
            lock (_cacheLock)
            {
                var urlToRemove = _cache.MinBy(x => x.LastUsed);
                if (urlToRemove != null)
                {
                    _cache.Remove(urlToRemove);
                }
            }
        }

        public void AddItemToCache(ShortUrlClsExtended shortUrlCls)
        {
            lock (_cacheLock)
            {
                _cache.Add(shortUrlCls);
            }
        }
    }
}
