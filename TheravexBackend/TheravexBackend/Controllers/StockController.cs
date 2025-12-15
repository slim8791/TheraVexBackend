using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheravexBackend.Data;
using TheravexBackend.Models;

namespace TheravexBackend.Controllers
{
    [ApiController]
    [Route("api/stock")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("entree")]
        public async Task<IActionResult> Entree(int articleId, int quantite)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null) return NotFound();

            article.Stock += quantite;

            _context.StockMouvements.Add(new StockMouvement
            {
                ArticleId = articleId,
                Quantite = quantite,
                Type = TypeMouvement.Entree
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("sortie")]
        public async Task<IActionResult> Sortie(int articleId, int quantite)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null || article.Stock < quantite)
                return BadRequest("Stock insuffisant");

            article.Stock -= quantite;

            _context.StockMouvements.Add(new StockMouvement
            {
                ArticleId = articleId,
                Quantite = quantite,
                Type = TypeMouvement.Sortie
            });

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
