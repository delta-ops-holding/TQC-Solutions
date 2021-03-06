using DatabaseAccess.Managers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmerald.Controllers
{
    [ApiController]
    [Route("v2/clan")]
    [EnableCors("TQC-Policy")]
    public class ClanV2Controller : ControllerBase
    {
        private ClanService Service { get; } = new ClanService();

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllClans()
        {
            try
            {
                var result = await Service.GetClansAsync();

                if (result == null)
                {
                    return BadRequest();
                }

                if (!result.Any())
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new Exception("Something went wrong while processing the request.") { Source = "Attempting to get clan information." });
            }
        }

        [Route("{id:int:min(1)}")]
        [HttpGet]
        public async Task<IActionResult> GetClan(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    var clan = await Service.GetClanAsync(identifier);

                    if (clan == null) return NotFound();

                    return Ok(clan);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return BadRequest(new Exception($"Something went wrong while processing the request."));
            }
        }

        [Route("authority")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthorities()
        {
            try
            {
                var result = await Service.GetMembersAsync();

                if (!result.Any()) return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new Exception("Something went wrong while processing the request."));
            }
        }

        [Route("authority/{id:int:min(1)}")]
        [HttpGet]
        public async Task<IActionResult> GetAuthority(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    var members = await Service.GetMembersAsync(identifier);

                    if (members == null) return NotFound();

                    if (!members.Any()) return NotFound();

                    return Ok(members);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return BadRequest(new Exception($"Something went wrong while processing the request."));
            }
        }

        [Route("platform")]
        [HttpGet]
        public async Task<IActionResult> GetAllPlatforms()
        {
            try
            {
                var platforms = await Service.GetClanPlatformsAsync();

                if (platforms == null) return NotFound();

                if (!platforms.Any()) return NotFound();

                return Ok(platforms);
            }
            catch (Exception)
            {
                return BadRequest(new Exception("Something went wrong while processing the request.") { Source = "" } );
            }
        }

        [Route("platform/{id:int:min(1)}")]
        [HttpGet]
        public async Task<IActionResult> GetPlatform(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    var platform = await Service.GetClanPlatformAsync(identifier);

                    if (platform == null) return NotFound();

                    return Ok(platform);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return BadRequest(new Exception($"Something went wrong while processing the request."));
            }
        }
    }
}
