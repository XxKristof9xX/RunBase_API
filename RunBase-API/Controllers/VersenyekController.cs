using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBase_API.Models;

namespace RunBase_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersenyekController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public VersenyekController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Versenyek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Versenyek>>> GetVersenyeks()
        {
            return await _context.Versenyeks.ToListAsync();
        }

        // GET: api/Versenyek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Versenyek>> GetVersenyek(int id)
        {
            var versenyek = await _context.Versenyeks.FindAsync(id);

            if (versenyek == null)
            {
                return NotFound();
            }

            return versenyek;
        }

        // PUT: api/Versenyek/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVersenyek(int id, Versenyek versenyek)
        {
            if (id != versenyek.VersenyId)
            {
                return BadRequest();
            }

            _context.Entry(versenyek).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VersenyekExists(id))
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

        // POST: api/Versenyek
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Versenyek>> PostVersenyek(Versenyek versenyek)
        {
            _context.Versenyeks.Add(versenyek);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VersenyekExists(versenyek.VersenyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVersenyek", new { id = versenyek.VersenyId }, versenyek);
        }

        // DELETE: api/Versenyek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVersenyek(int id)
        {
            var versenyek = await _context.Versenyeks.FindAsync(id);
            if (versenyek == null)
            {
                return NotFound();
            }

            _context.Versenyeks.Remove(versenyek);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VersenyekExists(int id)
        {
            return _context.Versenyeks.Any(e => e.VersenyId == id);
        }
    }
}
