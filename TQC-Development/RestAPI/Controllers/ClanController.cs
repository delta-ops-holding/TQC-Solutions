using System;
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
    [Route("v1/[controller]")]
    [ApiController]
    public class ClanController : ControllerBase
    {
        private protected IRepository<Clan> GetClanRepository = new ClanRepository();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clan>>> GetAll()
        {
            var clans = await GetClanRepository.GetAll() as List<Clan>;

            if (clans.Count == 0)
                return NotFound();
            else
                return clans;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Clan>> GetById(int id)
        {
            var clan = await GetClanRepository.GetById(id);

            if (clan.BaseId == 0)
                return NotFound();
            else
                return clan;
        }

        [HttpGet("authority")]
        public async Task<ActionResult<IEnumerable<ClanAuthority>>> GetAllClanAuthorities()
        {
            var authorities = await ((IClanAuthorityRepository)GetClanRepository).GetAll();

            if (authorities.Count() == 0)
                return NotFound();
            else
                return (List<ClanAuthority>)authorities;
        }

        [HttpGet("authority/{id:int}")]
        public async Task<ActionResult<IEnumerable<ClanAuthority>>> GetClanAuthorityById(int id)
        {
            var authorities = await ((IClanAuthorityRepository)GetClanRepository).GetById(id);

            if (authorities.Count() == 0)
                return NotFound();
            else
                return (List<ClanAuthority>)authorities;
        }

        [HttpGet("platform")]
        public async Task<ActionResult<IEnumerable<ClanPlatform>>> GetClanPlatforms()
        {
            var platforms = await ((IClanPlatformRepository)GetClanRepository).GetAll();

            if (platforms.Count() == 0)
                return NotFound();
            else
                return (List<ClanPlatform>)platforms;
        }

        [HttpGet("platform/{id:int}")]
        public async Task<ActionResult<ClanPlatform>> GetClanPlatformById(int id)
        {
            var platform = await ((IClanPlatformRepository)GetClanRepository).GetById(id);

            if (platform.BaseId == 0)
                return NotFound();
            else
                return platform;
        }
    }
}
