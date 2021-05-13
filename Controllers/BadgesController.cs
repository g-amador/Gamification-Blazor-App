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
    public class BadgesController : ControllerBase
    {
        private readonly BadgeContext _context;

        public BadgesController(BadgeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of badges.
        /// </summary>
        /// <returns>The list of all badges of the application.</returns>
        // GET: api/Badges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Badge>>> GetBadgeItems()
        {
            return await _context.BadgeItems.ToListAsync();
        }

        /// <summary>
        /// Get badge details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>All details of a badge.</returns>
        // GET: api/Badges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Badge>> GetBadge(long id)
        {
            var badge = await _context.BadgeItems.FindAsync(id);

            if (badge == null)
            {
                return NotFound();
            }

            return badge;
        }

        /// <summary>
        /// Update badge informations.
        /// 
        /// Update badge informations and icon path.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="badge"></param>
        /// <returns></returns>
        // PUT: api/Badges/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBadge(long id, Badge badge)
        {
            if (id != badge.Id)
            {
                return BadRequest();
            }

            _context.Entry(badge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BadgeExists(id))
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
        /// Add a new badge.
        /// 
        /// Create a new badge with the path to an icon representing the badge.
        /// </summary>
        /// <param name="badge"></param>
        /// <returns></returns>
        // POST: api/Badges
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Badge>> PostBadge(Badge badge)
        {
            _context.BadgeItems.Add(badge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBadge", new { id = badge.Id }, badge);
        }

        /// <summary>
        /// Delete a badge.
        /// 
        /// Delete a badge, all link to this badge by rules will be set to null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Badges/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Badge>> DeleteBadge(long id)
        {
            var badge = await _context.BadgeItems.FindAsync(id);
            if (badge == null)
            {
                return NotFound();
            }

            _context.BadgeItems.Remove(badge);
            await _context.SaveChangesAsync();

            return badge;
        }

        private bool BadgeExists(long id)
        {
            return _context.BadgeItems.Any(e => e.Id == id);
        }
    }
}
