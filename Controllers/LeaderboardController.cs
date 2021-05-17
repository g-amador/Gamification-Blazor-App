using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamificationApi.Models;
using GamificationApi.Models.Context;

namespace GamificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly PlayerContext _context;

        public LeaderboardController(PlayerContext context)
        {
            _context = context;
        }

        /*
        /// <summary>
        /// Get LeaderBoard. 
        /// </summary>
        /// <returns>The list of the five's best players of an application order by points.</returns>
        // GET: api/Badges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayerItems()
        {
            //TODO: implement method
            return await _context.PlayerItems.ToListAsync();
        }
        */
    }
}
