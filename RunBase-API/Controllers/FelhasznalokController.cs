using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBase_API.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using BCrypt.Net;

namespace RunBase_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FelhasznalokController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public FelhasznalokController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Felhasznalok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Felhasznalok>>> GetFelhasznaloks()
        {
            return await _context.Felhasznaloks.ToListAsync();
        }

        // GET: api/Felhasznalok/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Felhasznalok>> GetFelhasznalok(int id)
        {
            var felhasznalok = await _context.Felhasznaloks.FindAsync(id);

            if (felhasznalok == null)
            {
                return NotFound();
            }

            return felhasznalok;
        }

        // PUT: api/Felhasznalok/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFelhasznalok(int id, Felhasznalok felhasznalok)
        {
            if (id != felhasznalok.Id)
            {
                return BadRequest();
            }

            _context.Entry(felhasznalok).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FelhasznalokExists(id))
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

        // POST: api/Felhasznalok
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Felhasznalok>> PostFelhasznalok(Felhasznalok felhasznalok)
        {
            if (string.IsNullOrWhiteSpace(felhasznalok.Jelszo))
            {
                return BadRequest("A jelszó nem lehet üres!");
            }

            // Jelszó hashelése bcrypt segítségével
            felhasznalok.Jelszo = BCrypt.Net.BCrypt.HashPassword(felhasznalok.Jelszo);

            _context.Felhasznaloks.Add(felhasznalok);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFelhasznalok", new { id = felhasznalok.Id }, felhasznalok);
        }


        // DELETE: api/Felhasznalok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFelhasznalok(int id)
        {
            var felhasznalok = await _context.Felhasznaloks.FindAsync(id);
            if (felhasznalok == null)
            {
                return NotFound();
            }

            _context.Felhasznaloks.Remove(felhasznalok);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FelhasznalokExists(int id)
        {
            return _context.Felhasznaloks.Any(e => e.Id == id);
        }
    }
}
