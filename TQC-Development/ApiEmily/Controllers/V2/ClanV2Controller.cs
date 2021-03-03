using ApiEmily.Managers;
using ApiEmily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiEmily.Controllers.V2
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("v2/clan")]
    public class ClanV2Controller : ApiController
    {
        private ClanService Service { get; } = new ClanService();

        [Route("")]
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
                //return InternalServerError(ex);
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
                    var clan = await Service.GetClanAsync(identifier);

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
                var result = await Service.GetMembersAsync();

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
                return InternalServerError(new Exception($"Something went wrong while processing the request."));
            }
        }

        [Route("platform")]
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
                return InternalServerError(new Exception("Something went wrong while processing the request.") { Source = "" } );
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
                    ClanPlatform platform = await Service.GetClanPlatformAsync(identifier);

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
