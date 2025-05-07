using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBase_API.Models;
using Runbase_API.Models;

namespace RunBase_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersenytavController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public VersenytavController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Versenytav
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Versenytav>>> GetVersenytavs()
        {
            return await _context.Versenytavs.ToListAsync();
        }

        // GET: api/Versenytav/5
        [HttpGet("{versenyId}")]
        public async Task<ActionResult<IEnumerable<Versenytav>>> GetVersenytavok(int versenyId)
        {
            return await _context.Versenytavs
                .Where(vt => vt.VersenyId == versenyId)
                .ToListAsync();
        }

        // GET: api/Versenytav/5/10
        [HttpGet("{versenyId}/{tav}")]
        public async Task<ActionResult<Versenytav>> GetVersenytav(int versenyId, int tav)
        {
            var versenytav = await _context.Versenytavs
                .FirstOrDefaultAsync(vt => vt.VersenyId == versenyId && vt.Tav == tav);

            if (versenytav == null)
            {
                return NotFound();
            }

            return versenytav;
        }

        // PUT: api/Versenytav/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVersenytav(int id, Versenytav versenytav)
        {
            if (id != versenytav.Tav)
            {
                return BadRequest();
            }

            _context.Entry(versenytav).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VersenytavExists(id, versenytav.Tav))
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

        // POST: api/Versenytav
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Versenytav>> PostVersenytav(Versenytav versenytav)
        {
            _context.Versenytavs.Add(versenytav);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VersenytavExists(versenytav.VersenyId, versenytav.Tav))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetVersenytav),new { versenyId = versenytav.VersenyId, tav = versenytav.Tav }, versenytav);
        }

        // DELETE: api/versenytav/5/10 (versenyId: 5, tav: 10)
        [HttpDelete("{versenyId}/{tav}")]
        public async Task<IActionResult> DeleteVersenytav(int versenyId, int tav)
        {
            var versenytav = await _context.Versenytavs.FindAsync(new object[] { tav, versenyId });
            if (versenytav == null)
            {
                return NotFound();
            }

            _context.Versenytavs.Remove(versenytav);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool VersenytavExists(int versenyId, int tav)
        {
            return _context.Versenytavs.Any(e => e.VersenyId == versenyId && e.Tav == tav);
        }
    }
}
