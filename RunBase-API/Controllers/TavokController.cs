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
    public class TavokController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public TavokController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Tavok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tavok>>> GetTavoks()
        {
            return await _context.Tavoks.ToListAsync();
        }

        // GET: api/Tavok/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tavok>> GetTavok(int id)
        {
            var tavok = await _context.Tavoks.FindAsync(id);

            if (tavok == null)
            {
                return NotFound();
            }

            return tavok;
        }

        // PUT: api/Tavok/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTavok(int id, Tavok tavok)
        {
            if (id != tavok.TavId)
            {
                return BadRequest();
            }

            _context.Entry(tavok).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TavokExists(id))
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

        // POST: api/Tavok
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tavok>> PostTavok(Tavok tavok)
        {
            _context.Tavoks.Add(tavok);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTavok", new { id = tavok.TavId }, tavok);
        }

        // DELETE: api/Tavok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTavok(int id)
        {
            var tavok = await _context.Tavoks.FindAsync(id);
            if (tavok == null)
            {
                return NotFound();
            }

            _context.Tavoks.Remove(tavok);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TavokExists(int id)
        {
            return _context.Tavoks.Any(e => e.TavId == id);
        }
    }
}
