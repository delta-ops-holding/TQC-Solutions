﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Data;
using RestAPI.Data.Interfaces;
using RestAPI.Data.Objects;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        public IRepository<News> Repository { get; } = new NewsRepository();

        // GET: api/News
        [HttpGet]
        public ActionResult<List<News>> Get()
        {
            IEnumerable<News> news = Repository.GetAll();

            if (news == null) return NotFound();

            return news.ToList();
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public ActionResult<News> Get(int id)
        {
            if (id == 0) return NotFound();

            News data = Repository.GetById(id);

            if (data == null) return NotFound();

            return data;
        }

        // POST: api/News
        [HttpPost]
        public void Post([FromBody] News data)
        {
            if (data == null) return;
            Repository.Insert(data);
        }

        // PUT: api/News/5
        [HttpPut("{id}")]
        public void Put([FromBody] News data)
        {
            if (data == null || data.BaseId == 0) return;
            Repository.Update(data);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (id == 0) return;

            Repository.Delete(id);
        }
    }
}
