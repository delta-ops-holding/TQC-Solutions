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
    public class NewsController : ApiController
    {
        private NewsService Service { get; set; } = new NewsService();

        public async Task<IHttpActionResult> GetAllNews()
        {
            var result = await Service.GetAll();

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        public async Task<IHttpActionResult> GetNews(int id)
        {
            var result = await Service.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
