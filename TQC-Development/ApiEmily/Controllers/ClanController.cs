using ApiEmily.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiEmily.Controllers
{
    public class ClanController : ApiController
    {
        private ClanService Service { get; set; } = new ClanService();

        [Route("v1/clan")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllClans()
        {
            var result = await Service.GetAllClans();

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        [Route("v1/clan/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClan(int id)
        {
            var result = await Service.GetClanById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Route("v1/clan/authority")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAuthorities()
        {
            var result = await Service.GetAllAuthorities();

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        [Route("v1/clan/authority/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAuthority(int id)
        {
            var result = await Service.GetAuthoritiesById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Route("v1/clan/platform")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPlatforms()
        {
            var result = await Service.GetAllPlatforms();

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        [Route("v1/clan/platform/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPlatform(int id)
        {
            var result = await Service.GetPlatformById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
