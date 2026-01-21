using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheravexBackend.Data;
using TheravexBackend.Models;

namespace TheravexBackend.Controllers
{
    [ApiController]
    [Route("api/articles")]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var articles = await _context.Articles
                .Include(a => a.Tva)
                .Include(b => b.Lots)
                .ToListAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var articles = await _context.Articles
                .Where(a => a.Id == id)
                .Include(a => a.Tva)
                .Include(a => a.Lots)
                .ToListAsync();
            return Ok(articles);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return Ok(article);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Article article)
        {
            if (id != article.Id) return BadRequest();
            _context.Update(article);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
