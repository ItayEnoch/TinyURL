using System.Diagnostics;
using TinyURL.Models;

namespace TinyURL.Services
{
    public class RedirectService
    {
        private readonly HandleUrlService _handleUrlService;
        private readonly ICacheService _cacheService;
        private readonly List<ShortUrlClsExtended> _cache;
        private object _cacheLock = new object();
        private readonly int _maxCacheItems = 3;

        public RedirectService(HandleUrlService handleUrlService, ICacheService cacheService)
        {
            _handleUrlService = handleUrlService;
            _cacheService = cacheService;
            _cache = new List<ShortUrlClsExtended>();
            _cacheLock = new object();
            _cacheService = cacheService;
        }

        public async Task RequestRedirect(string url)
        {
            var fullUrl = string.Empty;
            ShortUrlClsExtended? urlItem = null;


            urlItem = _cacheService.GetCachedItem(url);

            if (urlItem != null)
            {
                fullUrl = urlItem.OriginalUrl;
            }
            else
            {
                int cacheCount = _cacheService.GetCachedItemCount();

                if (cacheCount < _maxCacheItems)
                {
                    fullUrl = await InsertToCacheAndReturnFullUrl(url, fullUrl);
                }
                else
                {
                    _cacheService.RemoveLastUsedFromCache();

                    fullUrl = await InsertToCacheAndReturnFullUrl(url, fullUrl);
                }
            }

            if (fullUrl != string.Empty)
            {
                Redirect(fullUrl);
            }
        }

        private async Task<string> InsertToCacheAndReturnFullUrl(string url, string fullUrl)
        {
            var newCls = await _handleUrlService.GetUrlClsAsync(url);

            if (newCls != null)
            {
                var newClsExtended = new ShortUrlClsExtended()
                {
                    Id = newCls.Id,
                    OriginalUrl = newCls.OriginalUrl,
                    ShortUrl = newCls.ShortUrl,
                    CreateDate = newCls.CreateDate,
                    LastUsed = DateTime.UtcNow
                };

                lock (_cacheLock)
                {
                    _cacheService.AddItemToCache(newClsExtended);
                }

                fullUrl = newCls.OriginalUrl;
            }

            return fullUrl;
        }

        public void Redirect(string fullUrl)
        {
            Process myProcess = new Process();

            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = fullUrl;
                myProcess.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Redirect");
            }
            finally
            {
                myProcess.Dispose();
            }
        }
    }
}
