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

        // GET: api/Felhasznalok/5 vagy nev 
        [HttpGet("{idOrUsername}")]
        public async Task<ActionResult<Felhasznalok>> GetFelhasznalok(string idOrUsername)
        {
            // Próbáljuk meg először ID-ként kezelni
            if (int.TryParse(idOrUsername, out int id))
            {
                var felhasznaloById = await _context.Felhasznaloks.FindAsync(id);
                if (felhasznaloById != null)
                {
                    return felhasznaloById;
                }
            }

            // Ha nem szám, akkor név alapján keresünk
            var felhasznaloByName = await _context.Felhasznaloks
                .FirstOrDefaultAsync(f => f.Nev == idOrUsername);

            if (felhasznaloByName == null)
            {
                return NotFound();
            }

            return felhasznaloByName;
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

            var existingUser = await _context.Felhasznaloks.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Csak azokat az értékeket frissítjük, amelyek nem nullák az új objektumban
            existingUser.VersenyzoId = felhasznalok.VersenyzoId ?? existingUser.VersenyzoId;
            existingUser.Nev = !string.IsNullOrEmpty(felhasznalok.Nev) ? felhasznalok.Nev : existingUser.Nev;
            existingUser.Tipus = !string.IsNullOrEmpty(felhasznalok.Tipus) ? felhasznalok.Tipus : existingUser.Tipus;

            // Ha a jelszó nincs megadva, akkor nem változtatunk rajta
            if (!string.IsNullOrEmpty(felhasznalok.Jelszo))
            {
                existingUser.Jelszo = felhasznalok.Jelszo;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Felhasznaloks.Any(e => e.Id == id))
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
        public async Task<ActionResult<Felhasznalok>> PostFelhasznalok([FromBody] Felhasznalok felhasznalo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Hibás adatok! Kérlek, töltsd ki helyesen a mezőket.");
            }

            if (string.IsNullOrWhiteSpace(felhasznalo.Nev) || felhasznalo.Nev.Length < 6)
            {
                return BadRequest("A felhasználónévnek legalább 6 karakter hosszúnak kell lennie!");
            }

            if (string.IsNullOrWhiteSpace(felhasznalo.Jelszo) || felhasznalo.Jelszo.Length < 8)
            {
                return BadRequest("A jelszónak legalább 8 karakter hosszúnak kell lennie!");
            }


            bool felhasznaloLetezik = await _context.Felhasznaloks
                .AnyAsync(f => f.Nev == felhasznalo.Nev);

            if (felhasznaloLetezik)
            {
                return Conflict("Ez a felhasználónév már foglalt! Kérlek, válassz másikat.");
            }

            if (string.IsNullOrWhiteSpace(felhasznalo.Jelszo))
            {
                return BadRequest("A jelszó nem lehet üres!");
            }

            try
            {
                felhasznalo.Jelszo = BCrypt.Net.BCrypt.HashPassword(felhasznalo.Jelszo);
                _context.Felhasznaloks.Add(felhasznalo);
                await _context.SaveChangesAsync();
                var routeValues = new { id = felhasznalo.Id };
                var getRoute = Url.Action("GetFelhasznaloById", "Felhasznalok", routeValues);
                if (string.IsNullOrEmpty(getRoute))
                {
                    return Ok(new { message = "Sikeres regisztráció!", id = felhasznalo.Id });
                }

                return CreatedAtAction("GetFelhasznalok", new { id = felhasznalo.Id }, felhasznalo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Szerverhiba történt: " + ex.Message);
            }
        }


            
        

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Dictionary<string, string> model)
        {
            if (!model.TryGetValue("Nev", out string? nev) || !model.TryGetValue("Jelszo", out string? jelszo))
            {
                return BadRequest("Hiányzó felhasználónév vagy jelszó.");
            }

            var user = await _context.Felhasznaloks.FirstOrDefaultAsync(u => u.Nev == nev);

            if (user == null || !BCrypt.Net.BCrypt.Verify(jelszo, user.Jelszo))
            {
                return Unauthorized("Hibás felhasználónév vagy jelszó.");
            }

            return Ok("Sikeres bejelentkezés!");
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
