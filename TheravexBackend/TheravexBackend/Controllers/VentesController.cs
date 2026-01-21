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
    public class VentesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ventes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vente>>> GetVente()
        {
            return await _context.Vente.ToListAsync();
        }

        // GET: api/Ventes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vente>> GetVente(int id)
        {
            var vente = await _context.Vente.FindAsync(id);

            if (vente == null)
            {
                return NotFound();
            }

            return vente;
        }

        // PUT: api/Ventes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVente(int id, Vente vente)
        {
            if (id != vente.VenteId)
            {
                return BadRequest();
            }

            _context.Entry(vente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenteExists(id))
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

        // POST: api/Ventes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vente>> PostVente(Vente vente)
        {
            _context.Vente.Add(vente);

            foreach(var item in vente.Lines)
            {
                var lot = await _context.Lot.FindAsync(item.LotId);
                if (lot != null && item.SellQuantity > 0)
                {
                    lot.Quantite -= item.SellQuantity;
                    _context.Entry(lot).State = EntityState.Modified;
                }

                var article = await _context.Articles.FindAsync(item.ArticleId);
                if (article != null && item.SellQuantity > 0)
                {
                    article.Stock -= item.SellQuantity;
                }
                _context.Entry(lot).State = EntityState.Modified;
                _context.Entry(article).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVente", new { id = vente.VenteId }, vente);
        }

        // DELETE: api/Ventes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVente(int id)
        {
            var vente = await _context.Vente.FindAsync(id);
            if (vente == null)
            {
                return NotFound();
            }

            _context.Vente.Remove(vente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VenteExists(int id)
        {
            return _context.Vente.Any(e => e.VenteId == id);
        }
    }
}
