using Microsoft.AspNetCore.Mvc;

namespace ApiEmerald.Controllers
{
    [ApiController]
    [Route("v1/clan")]
    public class ClanV1Controller : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public IActionResult ObsoleteClanRequestController()
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("{id:int:min(1)}")]
        [HttpGet]
        public IActionResult GetClan(int id)
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("authority")]
        [HttpGet]
        public IActionResult GetAllAuthorities()
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("authority/{id:int:min(1)}")]
        [HttpGet]
        public IActionResult GetAuthority(int id)
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("platform")]
        [HttpGet]
        public IActionResult GetAllPlatforms()
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("platform/{id:int:min(1)}")]
        [HttpGet]
        public IActionResult GetPlatform(int id)
        {
            return BadRequest("This Feature is no longer supported.");
        }
    }
}
