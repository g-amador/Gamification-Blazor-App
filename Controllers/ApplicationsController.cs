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
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ApplicationsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDTO>>> GetApplicationItems()
        {
            //return await _context.ApplicationItems.ToListAsync();            
            return await _context.ApplicationItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }*/

        /// <summary>
        /// Get application.
        /// 
        /// You must provide the apiKey and apiPassword in the http header.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The name, description and the key of an application.</returns>
        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDTO>> GetApplication(long id)
        {
            var application = await _context.ApplicationItems.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            //return application;
            return ItemToDTO(application);
        }

        /// <summary>
        /// Update application information.
        /// 
        /// You can update all informations of your application (including apiKey and apiPassword).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicationDTO"></param>
        /// <returns></returns>
        // PUT: api/Applications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(long id, ApplicationDTO applicationDTO)
        {
            if (id != applicationDTO.Id)
            {
                return BadRequest();
            }

            //_context.Entry(applicationDTO).State = EntityState.Modified;
            var application = await _context.ApplicationItems.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            application.Name = applicationDTO.Name;
            application.Description = applicationDTO.Description;

            /*try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ApplicationExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Create a new application.
        /// 
        /// This is the first thing you must do to use this API. Give a name, a description and a unique key and a secure password. 
        /// </summary>
        /// <param name="applicationDTO"></param>
        /// <returns>The application id.</returns>
        // POST: api/Applications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApplicationDTO>> PostApplication(ApplicationDTO applicationDTO)
        {
            var application = new Application
            {
                Description = applicationDTO.Description,
                Name = applicationDTO.Name
            };

            _context.ApplicationItems.Add(application);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetApplication", new { id = application.Id }, application);
            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, ItemToDTO(application));
        }

        /// <summary>
        /// Delete an application.
        /// 
        /// Be careful, when an application is deleted, all the attach resources are deleted in cascade!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        //public async Task<ActionResult<ApplicationDTO>> DeleteApplication(long id)
        public async Task<IActionResult> DeleteApplication(long id)
        {
            var application = await _context.ApplicationItems.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.ApplicationItems.Remove(application);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        /*private bool ApplicationExists(long id)
        {
            return _context.ApplicationItems.Any(e => e.Id == id);
        }*/

        private bool ApplicationExists(long id) =>
            _context.ApplicationItems.Any(e => e.Id == id);

        private static ApplicationDTO ItemToDTO(Application applicationItem) =>
            new ApplicationDTO
            {
                Id = applicationItem.Id,
                Name = applicationItem.Name,
                Description = applicationItem.Description                
            };
        }
}
