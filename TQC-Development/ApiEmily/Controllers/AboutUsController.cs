using ApiEmily.Managers;
using ApiEmily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiEmily.Controllers
{
    public class AboutUsController : ApiController
    {
        private AboutUsService Service { get; set; } = new AboutUsService();

        public async Task<IHttpActionResult> GetAllAboutUs()
        {
            var result = await Service.GetAll();

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        public async Task<IHttpActionResult> GetAboutUs(int id)
        {
            var result = await Service.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
