using GamificationAPI.Data;
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
    public class RulesController : ControllerBase
    {
        private readonly GamificationAPIContext _context;

        public RulesController(GamificationAPIContext context)
        {
            _context = context;
        }
        /*
        // GET: api/Rules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rule>>> GetRule()
        {
            return await _context.Rule.ToListAsync();
        }

        // GET: api/Rules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rule>> GetRule(long id)
        {
            var rule = await _context.Rule.FindAsync(id);

            if (rule == null)
            {
                return NotFound();
            }

            return rule;
        }

        // PUT: api/Rules/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRule(long id, Rule rule)
        {
            if (id != rule.Id)
            {
                return BadRequest();
            }

            _context.Entry(rule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuleExists(id))
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

        // POST: api/Rules
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Rule>> PostRule(Rule rule)
        {
            _context.Rule.Add(rule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRule", new { id = rule.Id }, rule);
        }

        // DELETE: api/Rules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rule>> DeleteRule(long id)
        {
            var rule = await _context.Rule.FindAsync(id);
            if (rule == null)
            {
                return NotFound();
            }

            _context.Rule.Remove(rule);
            await _context.SaveChangesAsync();

            return rule;
        }

        private bool RuleExists(long id)
        {
            return _context.Rule.Any(e => e.Id == id);
        }*/
    }
}
