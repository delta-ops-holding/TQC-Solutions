using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Managers.Interfaces;
using DatabaseAccess.Managers.V3;
using DatabaseAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmerald.Controllers
{
    [ApiController]
    [Route("v3/clan")]
    [EnableCors("TQC-Policy")]
    public class ClanV3Controller : ControllerBase
    {
        private IClanService ClanService { get; }

        public ClanV3Controller(IClanService service)
        {
            ClanService = service;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllClans()
        {
            try
            {
                var result = await ClanService.GetClansAsync();

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
                    var clan = await ClanService.GetClanAsync(identifier);

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
                var result = await ClanService.GetMembersAsync();

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
                    var member = await ClanService.GetMemberAsync(identifier);

                    if (member == null) return NotFound();

                    return Ok(member);
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
                var platforms = await ClanService.GetClanPlatformsAsync();

                if (platforms == null) return NotFound();

                if (!platforms.Any()) return NotFound();

                return Ok(platforms);
            }
            catch (Exception)
            {
                return BadRequest(new Exception("Something went wrong while processing the request.") { Source = "" });
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
                    var platform = await ClanService.GetClanPlatformAsync(identifier);

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
