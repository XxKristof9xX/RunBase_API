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
    public class JogosultsagsController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public JogosultsagsController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Jogosultsags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogosultsag>>> GetJogosultsags()
        {
            return await _context.Jogosultsags.ToListAsync();
        }

        // GET: api/Jogosultsags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jogosultsag>> GetJogosultsag(int id)
        {
            var jogosultsag = await _context.Jogosultsags.FindAsync(id);

            if (jogosultsag == null)
            {
                return NotFound();
            }

            return jogosultsag;
        }

        // PUT: api/Jogosultsags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJogosultsag(int id, Jogosultsag jogosultsag)
        {
            if (id != jogosultsag.FelhasznaloId)
            {
                return BadRequest();
            }

            _context.Entry(jogosultsag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JogosultsagExists(id))
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

        // POST: api/Jogosultsags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Jogosultsag>> PostJogosultsag(Jogosultsag jogosultsag)
        {
            _context.Jogosultsags.Add(jogosultsag);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (JogosultsagExists(jogosultsag.FelhasznaloId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetJogosultsag", new { id = jogosultsag.FelhasznaloId }, jogosultsag);
        }

        // DELETE: api/Jogosultsags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJogosultsag(int id)
        {
            var jogosultsag = await _context.Jogosultsags.FindAsync(id);
            if (jogosultsag == null)
            {
                return NotFound();
            }

            _context.Jogosultsags.Remove(jogosultsag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JogosultsagExists(int id)
        {
            return _context.Jogosultsags.Any(e => e.FelhasznaloId == id);
        }
    }
}
