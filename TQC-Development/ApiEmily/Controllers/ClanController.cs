using ApiEmily.Managers;
using ApiEmily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiEmily.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClanController : ApiController
    {
        private const string API_VERSION = "v1";

        private ClanService Service { get; } = new ClanService();

        [Route(API_VERSION + "/clan")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllClans()
        {
            try
            {
                var result = await Service.GetClansAsync();

                if (result.Count() == 0)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Something went wrong while handling the request."));
            }
        }

        [Route(API_VERSION + "/clan/{id:uint}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClan(uint id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    Clan clan = await Service.GetClanAsync(identifier);

                    if (clan == null) return NotFound();

                    return Ok(clan);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return InternalServerError(new Exception($"Something went wrong while handling the request."));
            }
        }

        [Route(API_VERSION + "/clan/authority")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAuthorities()
        {
            try
            {
                var result = await Service.GetMembersAsync();

                if (result.Count() == 0) return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Something went wrong while handling the request."));
            }
        }

        [Route(API_VERSION + "/clan/authority/{id:uint}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAuthority(uint id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    IEnumerable<Member> members = await Service.GetMembersAsync(identifier);

                    if (members == null) return NotFound();

                    if (members.Count().Equals(0)) return NotFound();

                    return Ok(members);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return InternalServerError(new Exception($"Something went wrong while handling the request."));
            }
        }

        [Route(API_VERSION + "/clan/platform")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPlatforms()
        {
            try
            {
                IEnumerable<ClanPlatform> platforms = await Service.GetClanPlatformsAsync();

                if (platforms == null) return NotFound();

                if (platforms.Count() == 0) return NotFound();

                return Ok(platforms);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Something went wrong while handling the request."));
            }
        }

        [Route(API_VERSION + "/clan/platform/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPlatform(int id)
        {
            try
            {
                if (id == 0) return BadRequest($"Parameter cannot be zero.");

                if (uint.TryParse(id.ToString(), out uint identifier))
                {
                    ClanPlatform platform = await Service.GetClanPlatformAsync(identifier);

                    if (platform == null) return NotFound();

                    return Ok(platform);
                }
                else
                    return BadRequest($"Parameter was in wrong data format.");

            }
            catch (Exception)
            {
                return InternalServerError(new Exception($"Something went wrong while handling the request."));
            }
        }
    }
}
