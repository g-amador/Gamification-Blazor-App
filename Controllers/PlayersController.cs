using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamificationApi.Models;

namespace GamificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayersController(PlayerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of players.
        /// </summary>
        /// <returns>The list of all players of an application.</returns>
        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayerItems()
        {
            return await _context.PlayerItems.ToListAsync();
        }


        /// <summary>
        /// Get player details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The detail of a player, including the numberOfPoints and list of all his badges</returns>
        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(long id)
        {
            var player = await _context.PlayerItems.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        /// <summary>
        /// Update player information.
        /// 
        /// Update basic informations of a player. The points and badges can only be updated by events.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        // PUT: api/Players/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(long id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Add a new player.
        /// 
        /// Create a new player with his basic informations. To manage points and badges, please go to events resources
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The new player's id.</returns>
        // POST: api/Players
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.PlayerItems.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }


        /// <summary>
        /// Delete a player
        /// 
        /// Delete a player, all events link to a player will be deleted in cascade.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(long id)
        {
            var player = await _context.PlayerItems.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.PlayerItems.Remove(player);
            await _context.SaveChangesAsync();

            return player;
        }

        private bool PlayerExists(long id)
        {
            return _context.PlayerItems.Any(e => e.Id == id);
        }
    }
}
