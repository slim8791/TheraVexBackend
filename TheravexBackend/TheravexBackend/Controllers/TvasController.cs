using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheravexBackend.Data;
using TheravexBackend.Models;

namespace TheravexBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TvasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tvas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tva>>> GetTvas()
        {
            return await _context.Tvas.ToListAsync();
        }

        // GET: api/Tvas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tva>> GetTva(int id)
        {
            var tva = await _context.Tvas.FindAsync(id);

            if (tva == null)
            {
                return NotFound();
            }

            return tva;
        }

        // PUT: api/Tvas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTva(int id, Tva tva)
        {
            if (id != tva.Id)
            {
                return BadRequest();
            }

            _context.Entry(tva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TvaExists(id))
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

        // POST: api/Tvas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tva>> PostTva(Tva tva)
        {
            _context.Tvas.Add(tva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTva", new { id = tva.Id }, tva);
        }

        // DELETE: api/Tvas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTva(int id)
        {
            var tva = await _context.Tvas.FindAsync(id);
            if (tva == null)
            {
                return NotFound();
            }

            _context.Tvas.Remove(tva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TvaExists(int id)
        {
            return _context.Tvas.Any(e => e.Id == id);
        }
    }
}
