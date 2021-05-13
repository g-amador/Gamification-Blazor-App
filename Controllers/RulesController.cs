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
    public class RulesController : ControllerBase
    {
        private readonly RuleContext _context;

        public RulesController(RuleContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of rules.
        /// </summary>
        /// <returns>The list of all rules of an application.</returns>
        // GET: api/Rules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rule>>> GetRuleItems()
        {
            return await _context.RuleItems.ToListAsync();
        }

        /// <summary>
        /// Get rule details.        
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The details of a rule.</returns>
        // GET: api/Rules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rule>> GetRule(long id)
        {
            var rule = await _context.RuleItems.FindAsync(id);

            if (rule == null)
            {
                return NotFound();
            }

            return rule;
        }

        /// <summary>
        /// Update rule informations.
        /// 
        /// Update a rule, this doesn't change badges and points give to players on past events.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add a new rule.
        /// 
        /// Add a new rule, the badgeId is optional, the numberOfPoints is required but he can be set to 0. 
        /// </summary>
        /// <param name="rule"></param>
        /// <returns>The new rule's id.</returns>
        // POST: api/Rules
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Rule>> PostRule(Rule rule)
        {
            _context.RuleItems.Add(rule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRule", new { id = rule.Id }, rule);
        }

        /// <summary>
        /// Delete a rule.
        /// 
        /// Delete a rule, this doesn't remove badges and points give to players on past events.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Rules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rule>> DeleteRule(long id)
        {
            var rule = await _context.RuleItems.FindAsync(id);
            if (rule == null)
            {
                return NotFound();
            }

            _context.RuleItems.Remove(rule);
            await _context.SaveChangesAsync();

            return rule;
        }

        private bool RuleExists(long id)
        {
            return _context.RuleItems.Any(e => e.Id == id);
        }
    }
}
