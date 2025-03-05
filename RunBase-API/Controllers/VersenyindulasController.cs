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
    public class VersenyindulasController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public VersenyindulasController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Versenyindulas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Versenyindulas>>> GetVersenyindulas()
        {
            return await _context.Versenyindulas.ToListAsync();
        }

        // GET: api/Versenyindulas/5
        [HttpGet("{versenyzoId}")]
        public async Task<ActionResult<List<Versenyindulas>>> GetVersenyindulas(int versenyzoId)
        {
            var versenyindulasok = await _context.Versenyindulas
                .Where(v => v.VersenyzoId == versenyzoId)
                .ToListAsync();

            if (versenyindulasok == null || versenyindulasok.Count == 0)
            {
                return NotFound("Nem található versenyindulás ezzel a VersenyzoId-val.");
            }

            return Ok(versenyindulasok);
        }

        // PUT: api/Versenyindulas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVersenyindulas(int id, Versenyindulas versenyindulas)
        {
            if (id != versenyindulas.VersenyzoId)
            {
                return BadRequest();
            }

            _context.Entry(versenyindulas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VersenyindulasExists(id))
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

        // POST: api/Versenyindulas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Versenyindulas>> PostVersenyindulas(Versenyindulas versenyindulas)
        {
            _context.Versenyindulas.Add(versenyindulas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VersenyindulasExists(versenyindulas.VersenyzoId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVersenyindulas", new { id = versenyindulas.VersenyzoId }, versenyindulas);
        }

        // DELETE: api/Versenyindulas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVersenyindulas(int id)
        {
            var versenyindulas = await _context.Versenyindulas.FindAsync(id);
            if (versenyindulas == null)
            {
                return NotFound();
            }

            _context.Versenyindulas.Remove(versenyindulas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VersenyindulasExists(int id)
        {
            return _context.Versenyindulas.Any(e => e.VersenyzoId == id);
        }
    }
}
