using DataAccessLibrary.Repositories.Abstractions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmerald.Controllers
{
    [ApiController]
    [Route("v4/clan")]
    [EnableCors("TQC-Policy")]
    public class ClanV4Controller : ControllerBase
    {
        private readonly IClanRepository _repository;

        public ClanV4Controller(IClanRepository repository)
        {
            _repository = repository;
        }

        [Route("{id:int:min(1)}")]
        [HttpGet]
        public async Task<IActionResult> GetClan(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            return Ok(result);

            //try
            //{
            //    if (id == 0) return BadRequest($"Parameter cannot be zero.");

            //    if (uint.TryParse(id.ToString(), out uint identifier))
            //    {
            //        var clan = await _repository.GetByIdAsync(id);

            //        if (clan == null) return NotFound();

            //        return Ok(clan);
            //    }
            //    else
            //        return BadRequest($"Parameter was in wrong data format.");

            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(new Exception($"Something went wrong while processing the request." + ex.Message));
            //}
        }
    }
}
