using ApiEmily.Managers.V3;
using ApiEmily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiEmily.Controllers.V3
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("v3/clan")]
    public class ClanV3Controller : ApiController
    {
        private readonly ClanV3Service _clanService = new ClanV3Service();

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllClans()
        {
            try
            {
                var result = await _clanService.GetClansAsync();

                if (result.Count() == 0)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Something went wrong while processing the request.") { Source = "Attempting to get clan information." });
            }
        }

        [Route("{id:int:min(1)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClan(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    var clan = await _clanService.GetClanAsync(identifier);

                    if (clan == null) return NotFound();

                    return Ok(clan);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return InternalServerError(new Exception($"Something went wrong while processing the request."));
            }
        }

        [Route("authority")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAuthorities()
        {
            try
            {
                var result = await _clanService.GetMembersAsync();

                if (result.Count() == 0) return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Something went wrong while processing the request."));
            }
        }

        [Route("authority/{id:int:min(1)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAuthority(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    var member = await _clanService.GetMemberAsync(identifier);

                    if (member == null) return NotFound();

                    return Ok(member);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return InternalServerError(new Exception($"Something went wrong while processing the request."));
            }
        }

        [Route("platform")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPlatforms()
        {
            try
            {
                IEnumerable<ClanPlatform> platforms = await _clanService.GetClanPlatformsAsync();

                if (platforms == null) return NotFound();

                if (platforms.Count() == 0) return NotFound();

                return Ok(platforms);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Something went wrong while processing the request.") { Source = "" });
            }
        }

        [Route("platform/{id:int:min(1)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPlatform(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    ClanPlatform platform = await _clanService.GetClanPlatformAsync(identifier);

                    if (platform == null) return NotFound();

                    return Ok(platform);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return InternalServerError(new Exception($"Something went wrong while processing the request."));
            }
        }
    }
}
