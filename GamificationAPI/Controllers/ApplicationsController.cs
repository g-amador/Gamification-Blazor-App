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
    public class ApplicationsController : ControllerBase
    {
        private readonly GamificationAPIContext _context;

        public ApplicationsController(GamificationAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of all applications.
        /// </summary>
        /// <returns></returns>
        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDTO>>> GetApplication()
        {
            //return await _context.Application.ToListAsync();
            return await _context.Application
                .Select(a => ApplicationToDTO(a))
                .ToListAsync();            
        }

        /// <summary>
        /// Get application.
        /// 
        /// You must provide the apiKey and apiPassword in the http header.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <returns>The name, description and the key of an application.</returns>
        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDTO>> GetApplication(long id, [FromHeader] string apiKey, [FromHeader] string apiSecret)
        {
            var application = await _context.Application.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            if (IsNotAuthToModify(application, apiKey, apiSecret))
            {
                return BadRequest();
            }

            return ApplicationToDTO(application);
        }

        /// <summary>
        /// Update application information.
        /// 
        /// You can update all informations of your application (including apiKey and apiPassword).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="application"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <returns></returns>
        // PUT: api/Applications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(long id, Application application, [FromHeader] string apiKey, [FromHeader] string apiSecret)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            bool isNotUniqueApiKey = await _context.Application.AnyAsync(a => a.Id != id && a.ApiKey == apiKey);
            bool isNotAuthToModify = await _context.Application.AnyAsync(a => a.Id == id && (a.ApiKey != apiKey || a.ApiSecret != apiSecret));
            if (isNotUniqueApiKey || isNotAuthToModify)
            {
                return BadRequest();
            }

            //TODO: update locally and on database associations
            /*
            foreach (Badge b in application.Badges)
            {
                b.Application = application;                
            }
            foreach (Event e in application.Events)
            {
                e.Application = application;
            }
            foreach (Player p in application.Players)
            {
                p.Application = application;
            }
            foreach (Rule r in application.Rules)
            {
                r.Application = application;
            }
            */

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ApplicationExists(id))
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Create a new application.
        /// 
        /// This is the first thing you must do to use this API. Give a name, a description and a unique key and a secure password. 
        /// </summary>
        /// <param name="application"></param>
        /// <returns>The application id.</returns>
        // POST: api/Applications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //public async Task<ActionResult<Application>> PostApplication(Application application)
        public async Task<ActionResult<string>> PostApplication(Application application)
        {
            bool isNotUniqueApiKey = await _context.Application.AnyAsync(a => a.ApiKey == application.ApiKey);
            if (isNotUniqueApiKey)
            {
                return BadRequest();
            }

            _context.Application.Add(application);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetApplication", new { id = application.Id }, application);
            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, new { Status = "created", url = "/application/" + application.Id });
        }

        /// <summary>
        /// Delete an application.
        /// 
        /// Be careful, when an application is deleted, all the attach resources are deleted in cascade!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <returns></returns>
        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        //public async Task<ActionResult<Application>> DeleteApplication(long id)
        public async Task<ActionResult> DeleteApplication(long id, [FromHeader] string apiKey, [FromHeader] string apiSecret)
        {
            var application = await _context.Application.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            if (IsNotAuthToModify(application, apiKey, apiSecret)) 
            {
                return BadRequest();
            }

            //TODO: update locally and on database associations
            /*
            application.Badges.Clear();
            application.Events.Clear();
            application.Players.Clear();
            application.Rules.Clear();
            */

            _context.Application.Remove(application);
            await _context.SaveChangesAsync();

            //return application;
            return Ok();
        }

        private bool ApplicationExists(long id) =>
            _context.Application.Any(a => a.Id == id);

        private static ApplicationDTO ApplicationToDTO(Application applicationItem) =>
            new ApplicationDTO
            {
                Name = applicationItem.Name,
                Description = applicationItem.Description,
                ApiKey = applicationItem.ApiKey
            };

        private bool IsNotAuthToModify(Application application, string apiKey, string apiSecret)
        {
            return (application.ApiKey != apiKey || application.ApiSecret != apiSecret);
        }
    }
}
