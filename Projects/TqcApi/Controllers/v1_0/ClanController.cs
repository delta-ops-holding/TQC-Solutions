using DataAccessLibrary.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TqcApi.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class ClanController : ControllerBase
    {
        private readonly IClanRepository _clanRepository;

        public ClanController(IClanRepository clanRepository)
        {
            _clanRepository = clanRepository;
        }

        // GET: api/<ClanController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clans = await _clanRepository.GetAsync();

                if (clans == null) return BadRequest("Request couldn't be handled at this time.");

                if (!clans.Any()) return NotFound("No clans were found.");

                return Ok(clans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ClanController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
