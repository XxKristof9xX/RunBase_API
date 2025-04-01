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
    public class ForumController : ControllerBase
    {
        private readonly RunBaseDbContext _context;

        public ForumController(RunBaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Forum
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forum>>> GetForumBejegyzesek()
        {
            return await _context.ForumBejegyzesek.ToListAsync();
        }

        // GET: api/Forum/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Forum>> GetForum(int id)
        {
            var forum = await _context.ForumBejegyzesek.FindAsync(id);

            if (forum == null)
            {
                return NotFound();
            }

            return forum;
        }

        // PUT: api/Forum/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForum(int id, Forum forum)
        {
            if (id != forum.Id)
            {
                return BadRequest();
            }

            _context.Entry(forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(id))
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

        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Forum>> PostForum(Forum forum)
        {
            _context.ForumBejegyzesek.Add(forum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForum", new { id = forum.Id }, forum);
        }

        // DELETE: api/Forum/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            var forum = await _context.ForumBejegyzesek.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            _context.ForumBejegyzesek.Remove(forum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumExists(int id)
        {
            return _context.ForumBejegyzesek.Any(e => e.Id == id);
        }
    }
}
