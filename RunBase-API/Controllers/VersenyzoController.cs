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
    public class VersenyzoController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public VersenyzoController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Versenyzo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Versenyzo>>> GetVersenyzos()
        {
            return await _context.Versenyzos.ToListAsync();
        }

        // GET: api/Versenyzo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Versenyzo>> GetVersenyzo(int id)
        {
            var versenyzo = await _context.Versenyzos.FindAsync(id);

            if (versenyzo == null)
            {
                return NotFound();
            }

            return versenyzo;
        }

        // GET: api/getByTaj/
        [HttpGet("getByTaj/{tajszam}")]
        public async Task<ActionResult<int>> GetVersenyzoIdByTaj(string tajszam)
        {
            var versenyzo = await _context.Versenyzos
                .Where(v => v.TajSzam == tajszam)
                .Select(v => v.VersenyzoId)
                .FirstOrDefaultAsync();

            if (versenyzo == 0)
            {
                return NotFound(new { message = "A megadott TAJ számmal nem található versenyző." });
            }

            return Ok(new { versenyzoId = versenyzo });
        }

        [HttpPut("addVersenyzo")]
        public async Task<IActionResult> AddVersenyzoToUser([FromBody] AddVersenyzoRequest request)
        {
            var versenyzoId = await _context.Versenyzos
                .Where(v => v.TajSzam == request.TajSzam)
                .Select(v => v.VersenyzoId)
                .FirstOrDefaultAsync();

            if (versenyzoId == 0)
            {
                return NotFound(new { message = "A megadott TAJ számmal nem található versenyző." });
            }

            var user = await _context.Felhasznaloks.FindAsync(request.FelhasznaloId);
            if (user == null)
            {
                return NotFound(new { message = "A megadott felhasználó nem található." });
            }

            user.VersenyzoId = versenyzoId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "A versenyző sikeresen hozzá lett adva a felhasználóhoz.", versenyzoId });
        }

        public class AddVersenyzoRequest
        {
            public string TajSzam { get; set; }
            public int FelhasznaloId { get; set; }
        }


        // PUT: api/Versenyzo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVersenyzo(int id, Versenyzo versenyzo)
        {
            if (id != versenyzo.VersenyzoId)
            {
                return BadRequest();
            }

            _context.Entry(versenyzo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VersenyzoExists(id))
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

        // POST: api/Versenyzo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Versenyzo>> PostVersenyzo(Versenyzo versenyzo)
        {
            _context.Versenyzos.Add(versenyzo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVersenyzo", new { id = versenyzo.VersenyzoId }, versenyzo);
        }

        // DELETE: api/Versenyzo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVersenyzo(int id)
        {
            var versenyzo = await _context.Versenyzos.FindAsync(id);
            if (versenyzo == null)
            {
                return NotFound();
            }

            _context.Versenyzos.Remove(versenyzo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VersenyzoExists(int id)
        {
            return _context.Versenyzos.Any(e => e.VersenyzoId == id);
        }
    }
}
