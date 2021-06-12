using GamificationAPI.Data;
using GamificationAPI.DTO;
using GamificationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly GamificationAPIContext _context;

        public PlayersController(GamificationAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of players.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns>The list of all players of an application.</returns>
        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayer([FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            try
            {
                Application application = await _context.Application
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.ApiKey == apiKey && a.ApiPassword == apiPassword);

                if (application == null)
                {
                    return BadRequest();
                }

                return await _context.Player
                    .Include(p => p.Application)
                    .Where(p => p.Application.ApiKey == apiKey && p.Application.ApiPassword == apiPassword)
                    .Select(p => PlayerToDTO(p))
                    .ToListAsync();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Get player details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns>The detail of a player, including the numberOfPoints and list of all his badges</returns>
        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(long id, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            var player = await _context.Player
                .Include(p => p.Application)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            if (player.Application.IsNotAuth(player.Application, apiKey, apiPassword))
            {
                return BadRequest();
            }

            return PlayerToDTO(player);
        }

        /// <summary>
        /// Update player information.
        /// 
        /// Update basic informations of a player. The points and badges can only be updated by events.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns></returns>
        // PUT: api/Players/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(long id, Player player, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            var application = await _context.Application
                .FirstAsync(a => a.ApiKey == apiKey && a.ApiPassword == apiPassword);

            if (application == null)
            {
                return BadRequest();
            }

            player.Application = application;            

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PlayerExists(id))
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Add a new player.
        /// 
        /// Create a new player with his basic informations. To manage points and badges, please go to events resources
        /// </summary>
        /// <param name="player"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns>The new player's id.</returns>
        // POST: api/Players
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            try
            {
                Application application = await _context.Application
                    .FirstOrDefaultAsync(a => a.ApiKey == apiKey && a.ApiPassword == apiPassword);

                if (application == null)
                {
                    return BadRequest();
                }

                player.Application = application;

                _context.Player.Add(player);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, new { Status = "created", url = "/player/" + player.Id });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a player
        /// 
        /// Delete a player, all events link to a player will be deleted in cascade.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns></returns>
        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(long id, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            var player = await _context.Player
                .Include(p => p.Application)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            if (player.Application.IsNotAuth(player.Application, apiKey, apiPassword))
            {
                return BadRequest();
            }

            var events = await _context.Event
                .Where(e => e.Player.Id == player.Id)
                .ToListAsync();

            foreach (var e in events) {
                _context.Event.Remove(e);
            }

            _context.Player.Remove(player);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PlayerExists(long id) => 
            _context.Player.Any(p => p.Id == id);

        private static PlayerDTO PlayerToDTO(Player player) =>
           new PlayerDTO
           {
               Id = player.Id,
               NickName = player.NickName,
               FirstName = player.FirstName,
               LastName = player.LastName,
               ProfilePicture = player.ProfilePicture,
               Email = player.Email,
               NumberOfPoints = player.NumberOfPoints,
               NumberOfCredits = player.NumberOfCredits,
               Badges = player.Badges
           };
    }
}
