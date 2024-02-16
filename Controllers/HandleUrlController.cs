using Microsoft.AspNetCore.Mvc;
using TinyURL.Models;
using TinyURL.Services;

namespace TinyURL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HandleUrlController : ControllerBase
    {
        private readonly HandleUrlService _handleUrlService;

        public HandleUrlController(HandleUrlService handleUrlService)
        {
            _handleUrlService = handleUrlService;
        }

        [HttpGet]
        [Route("GetUrls")]
        public async Task<List<ShortUrlCls>> GetUrls() => await _handleUrlService.GetAsync();

        [HttpGet]
        [Route("GetFullUrl")]
        public async Task<string> GetFullUrl(string shortUrl)
        {
            var fullUrl = await _handleUrlService.GetFullUrlAsync(shortUrl); 
            
            return fullUrl;
        }

        [HttpPost]
        [Route("GenerateShortUrl")]
        public async Task<IActionResult> GenerateShortUrl(string url)
        {
            var shortUrl = await _handleUrlService.CreateShortUrlAsync(url);

            return CreatedAtAction(nameof(GetUrls), new { id = shortUrl.Id }, shortUrl);
        }
    }
}
