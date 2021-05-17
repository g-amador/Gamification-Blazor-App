using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamificationApi.Models;
using GamificationApi.Models.DTO;
using GamificationApi.Models.Context;
using System.Net.Http.Headers;

namespace GamificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ApplicationsController(ApplicationContext context)
        {
            _context = context;
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
            var application = await _context.ApplicationItems.FindAsync(id);
            
            if (application == null)
            {
                return NotFound();
            }

            if (application.ApiKey != apiKey || application.ApiSecret != apiSecret)
            {
                return BadRequest();
            }                    

            //return application;
            return ApplicationToDTO(application);
        }

        /// <summary>
        /// Update application information.
        /// 
        /// You can update all informations of your application (including apiKey and apiPassword).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        // PUT: api/Applications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(long id, [FromHeader] string apiKey, [FromHeader] string apiSecret, Application application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }
            
            bool isNotUniqueApiKey = await _context.ApplicationItems.AnyAsync(a => a.Id != id && a.ApiKey == apiKey);
            bool isNotAuthToModify = await _context.ApplicationItems.AnyAsync(a => a.Id == id && (a.ApiKey != apiKey || a.ApiSecret != apiSecret));
            if (isNotUniqueApiKey || isNotAuthToModify) 
            {
                return BadRequest();
            }


            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ApplicationExists(id))
            {
                return NotFound();                
            }

            //TODO: Change badges, etc and all if needed
            /*
            _ = await BadgesController.PutBadge(apiKey, apiSecret);
            _ = await EventsController.PutEvent(apiKey, apiSecret);
            _ = await PlayersController.PutPlayer(apiKey, apiSecret);
            _ = await RulesController.PutRule(apiKey, apiSecret);
            */

            return NoContent();
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
        public async Task<ActionResult<String>> PostApplication(Application application)
        {
            bool isNotUniqueApiKey = await _context.ApplicationItems.AnyAsync(a => a.ApiKey == application.ApiKey);
            if (isNotUniqueApiKey)
            {
                return BadRequest();
            }

            _context.ApplicationItems.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application.Id);
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
        public async Task<IActionResult> DeleteApplication(long id, [FromHeader] string apiKey, [FromHeader] string apiSecret)
        {
            var application = await _context.ApplicationItems.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            if (application.ApiKey != apiKey || application.ApiSecret != apiSecret)
            {
                return BadRequest();
            }

            //TODO: implement latter
           /*
           _ = await BadgesController.DeleteBadge(apiKey, apiSecret);
            _ = await EventsController.DeleteEvent(apiKey, apiSecret);
            _ = await PlayersController.DeletePlayer(apiKey, apiSecret);
            _ = await RulesController.DeleteRule(apiKey, apiSecret);
           */

            _context.ApplicationItems.Remove(application);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool ApplicationExists(long id) =>
            _context.ApplicationItems.Any(e => e.Id == id);

        /*private bool ApplicationExistsAndAuthValid(long id, string apiKey, string apiSecret) =>
            _context.ApplicationItems.Any(e => e.Id == id && e.ApiKey == apiKey && e.ApiSecret == apiSecret);*/

        private static ApplicationDTO ApplicationToDTO(Application applicationItem) =>
            new ApplicationDTO
            {
                Name = applicationItem.Name,
                Description = applicationItem.Description,
                ApiKey = applicationItem.ApiKey
            };
        }
}
