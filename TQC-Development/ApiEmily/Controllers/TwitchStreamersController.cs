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
    public class TwitchStreamersController : ApiController
    {
        private TwitchStreamService Service { get; set; } = new TwitchStreamService();

        public async Task<IHttpActionResult> GetAllStreamers()
        {
            var result = await Service.GetAll();

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        public async Task<IHttpActionResult> GetStreamer(int id)
        {
            var result = await Service.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
