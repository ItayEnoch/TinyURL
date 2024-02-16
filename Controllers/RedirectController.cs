using Microsoft.AspNetCore.Mvc;
using TinyURL.Models;
using TinyURL.Services;

namespace TinyURL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedirectController : ControllerBase
    {
        private readonly RedirectService _redirectService;

        public RedirectController(RedirectService redirectService)
        {
            _redirectService = redirectService;
        }

        [HttpGet]
        [Route("RedirectToPage")]
        public async Task<ActionResult> RedirectToPage(string shortUrl)
        {
            await _redirectService.RequestRedirect(shortUrl);

            return Ok();
        }
    }
}
