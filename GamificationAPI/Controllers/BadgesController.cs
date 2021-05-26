using GamificationAPI.Data;
using GamificationAPI.DTO;
using GamificationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgesController : ControllerBase
    {
        private readonly GamificationAPIContext _context;

        public BadgesController(GamificationAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of badges.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns>The list of all badges of the application.</returns>
        // GET: api/Badges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadgeDTO>>> GetBadge([FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            //return await _context.Badge.ToListAsync();
            return await _context.Badge
                .Select(b => b)
                .Where(b => b.Application.ApiKey == apiKey && b.Application.ApiPassword == apiPassword)
                .Select(b => BadgeToDTO(b))
                .ToListAsync();
        }

        /// <summary>
        /// Get badge details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns>All details of a badge.</returns>
        // GET: api/Badges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BadgeDTO>> GetBadge(long id, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            var badge = await _context.Badge.FindAsync(id);

            if (badge == null)
            {
                return NotFound();
            }

            if (badge.Application.IsNotAuthToModify(badge.Application, apiKey, apiPassword))
            {
                return BadRequest();
            }

            return BadgeToDTO(badge); 
        }

        /// <summary>
        /// Update badge informations.
        /// 
        /// Update badge informations and icon path.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="badge"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiPassword"></param>
        /// <returns></returns>
        // PUT: api/Badges/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBadge(long id, Badge badge, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            if (id != badge.Id)
            {
                return BadRequest();
            }

            if (badge.Application.IsNotAuthToModify(badge.Application, apiKey, apiPassword))
            {
                return BadRequest();
            }

            badge.Application.Badges.Remove(badge.Application.Badges.First(b => b.Id == id));
            badge.Application.Badges.Add(badge);

            _context.Entry(badge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BadgeExists(id))
            {
                return NotFound();
            }

            return Ok();
        }

        // POST: api/Badges
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Badge>> PostBadge(Badge badge, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            Application application = await _context.Application.FirstAsync(a => a.ApiKey == apiKey && a.ApiPassword == apiPassword);

            if (application == null)
            {
                return BadRequest();
            }

            badge.Application = application;
            application.Badges.Add(badge);

            _context.Badge.Add(badge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBadge", new { id = badge.Id }, badge);
        }

        // DELETE: api/Badges/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Badge>> DeleteBadge(long id, [FromHeader] string apiKey, [FromHeader] string apiPassword)
        {
            var badge = await _context.Badge.FindAsync(id);
            if (badge == null)
            {
                return NotFound();
            }

            if (badge.Application.IsNotAuthToModify(badge.Application, apiKey, apiPassword))
            {
                return BadRequest();
            }

            badge.Application.Badges.Remove(badge);

            _context.Badge.Remove(badge);
            await _context.SaveChangesAsync();

            return badge;
        }

        private bool BadgeExists(long id) => 
            _context.Badge.Any(b => b.Id == id);

        private static BadgeDTO BadgeToDTO(Badge badge) =>
            new BadgeDTO
            {
                Id = badge.Id,
                Name = badge.Name,
                Description = badge.Description,
                Icon = badge.Icon
            };
    }
}
