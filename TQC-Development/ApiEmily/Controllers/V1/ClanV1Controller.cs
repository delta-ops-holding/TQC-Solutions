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
        [Route]
        public IHttpActionResult ObsoleteController()
        {
            return BadRequest("Controller is deprecated. Please use a newer version.");
        }
    }
}
