using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        [HttpGet]
        public IActionResult PublicApi()
        {
            var list = new[] {
            new { Id = 1, Name = "This endpoint can be accessed publicly" },
            new { Id = 2, Name = "So can this endpoint" }
            }.ToList();

            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        public IActionResult PrivateApi()
        {
            var list = new[] {
            new { Id = 1, Name = "This endpoint is restricted" },
            new { Id = 2, Name = "You need to login to see this" }
            }.ToList();

            return Ok(list);
        }

    }
}
