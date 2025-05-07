using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBase_API.Models;
using System.IO;

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

        // GET: api/Versenyek - Versenyek listázása képpel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetVersenyeks()
        {
            var versenyek = await _context.Versenyeks.ToListAsync();

            return versenyek.Select(v => new
            {
                v.VersenyId,
                v.Nev,
                v.Helyszin,
                v.Datum,
                v.Leiras,
                v.MaxLetszam,
                Kep = v.Kep != null ? Convert.ToBase64String(v.Kep) : null
            }).ToList();
        }

        // GET: api/Versenyek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetVersenyek(int id)
        {
            var versenyek = await _context.Versenyeks.FindAsync(id);

            if (versenyek == null)
            {
                return NotFound();
            }

            return new
            {
                versenyek.VersenyId,
                versenyek.Nev,
                versenyek.Helyszin,
                versenyek.Datum,
                versenyek.Leiras,
                versenyek.MaxLetszam,
                Kep = versenyek.Kep != null ? Convert.ToBase64String(versenyek.Kep) : null
            };
        }

        // POST: api/Versenyek
        [HttpPost]
        public async Task<ActionResult<Versenyek>> PostVersenyek([FromForm] VersenyCreateDto model)
        {
            var verseny = new Versenyek
            {
                Nev = model.Nev,
                Helyszin = model.Helyszin,
                Datum = model.Datum,
                Leiras = model.Leiras,
                MaxLetszam = model.MaxLetszam,
                Kep = model.Kep != null ? ConvertFileToBytes(model.Kep) : null
            };

            _context.Versenyeks.Add(verseny);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVersenyek", new { id = verseny.VersenyId }, verseny);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutVersenyek(int id, [FromForm] VersenyUpdateDto versenyUpdateDto)
        {
            var verseny = await _context.Versenyeks.FindAsync(id);
            if (verseny == null)
            {
                return NotFound();
            }
            verseny.Nev = versenyUpdateDto.Nev;
            verseny.Helyszin = versenyUpdateDto.Helyszin;
            verseny.Datum = versenyUpdateDto.Datum;
            verseny.Leiras = versenyUpdateDto.Leiras;
            verseny.MaxLetszam = versenyUpdateDto.MaxLetszam;
            if (versenyUpdateDto.Kep != null)
            {
                using var ms = new MemoryStream();
                await versenyUpdateDto.Kep.CopyToAsync(ms);
                verseny.Kep = ms.ToArray();
            }

            _context.Entry(verseny).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Versenyeks.Any(e => e.VersenyId == id))
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

        // DELETE: api/Versenyek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVersenyek(int id)
        {
            var versenyek = await _context.Versenyeks.FindAsync(id);
            if (versenyek == null)
            {
                return NotFound();
            }

            var tavok = _context.Versenytavs.Where(v => v.VersenyId == id);
            _context.Versenytavs.RemoveRange(tavok);
            var indulok = _context.Versenyindulas.Where(v => v.VersenyId == id);
            _context.Versenyindulas.RemoveRange(indulok);

            _context.Versenyeks.Remove(versenyek);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VersenyekExists(int id)
        {
            return _context.Versenyeks.Any(e => e.VersenyId == id);
        }

        private static byte[] ConvertFileToBytes(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }

    public class VersenyCreateDto
    {
        public string Nev { get; set; } = null!;
        public string Helyszin { get; set; } = null!;
        public DateOnly Datum { get; set; }
        public string Leiras { get; set; } = null!;
        public int MaxLetszam { get; set; }
        public IFormFile? Kep { get; set; }
    }

    public class VersenyUpdateDto
    {
        public string Nev { get; set; } = null!;
        public string Helyszin { get; set; } = null!;
        public DateOnly Datum { get; set; }
        public string Leiras { get; set; } = null!;
        public int MaxLetszam { get; set; }
        public IFormFile? Kep { get; set; }
    }
}
