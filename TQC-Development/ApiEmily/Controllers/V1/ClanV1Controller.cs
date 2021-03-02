using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiEmily.Controllers.V1
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("v1/clan")]
    public class ClanV1Controller : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult ObsoleteClanRequestController()
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("{id:int:min(1)}")]
        [HttpGet]
        public IHttpActionResult GetClan(int id)
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("authority")]
        [HttpGet]
        public IHttpActionResult GetAllAuthorities()
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("authority/{id:int:min(1)}")]
        [HttpGet]
        public IHttpActionResult GetAuthority(int id)
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("platform")]
        [HttpGet]
        public IHttpActionResult GetAllPlatforms()
        {
            return BadRequest("This Feature is no longer supported.");
        }

        [Route("platform/{id:int:min(1)}")]
        [HttpGet]
        public IHttpActionResult GetPlatform(int id)
        {
            return BadRequest("This Feature is no longer supported.");
        }
    }
}
