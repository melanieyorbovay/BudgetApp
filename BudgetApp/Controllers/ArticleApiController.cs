using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleApiController : ControllerBase
    {
        private readonly DataContext _context;

        public ArticleApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetArticles()
        {
            var articles = _context.Articles
                .OrderBy(a => a.NomArticle)
                .ToList();
            return Ok(articles);
        }
        [HttpPost]
        public IActionResult AjouterArticle([FromBody] Article article)
        {
            //Vérification du doublon
            bool existe = _context.Articles
                .Any(a => a.NomArticleNormalized == article.NomArticle.Trim().ToLower());

            if (existe)
            {
                return Conflict($"L'article \"{article.NomArticle}\" existe déjà.");
            }

            var nouvel = new Article
            {
                NomArticle = article.NomArticle.Trim(),
                Unite = article.Unite.Trim(),
                IdCategorie = article.IdCategorie
            };

            _context.Articles.Add(nouvel);
            _context.SaveChanges();

            //L'article retourné contient le vrai ID généré par la base de données SQL Server.
            return Ok(nouvel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifierArticle(int id, [FromBody] Article article)
        {
            if (string.IsNullOrWhiteSpace(article.NomArticle) || string.IsNullOrWhiteSpace(article.Unite))
            {
                return BadRequest("Le nom et l'unité sont obligatoires.");
            }
            var existant = await _context.Articles.FindAsync(id);
            if (existant == null)
            {
                return NotFound();
            }
            var nom = article.NomArticle.Trim();
            var unite = article.Unite.Trim();

            bool doublon = await _context.Articles
                .AnyAsync(a => a.IdArticle != id && a.NomArticleNormalized == nom.ToLower());
            if (doublon)
            {
                return Conflict($"L'article \"{nom}\" existe déjà.");
            }

            existant.NomArticle = nom;
            existant.Unite = unite;
            existant.IdCategorie = article.IdCategorie;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Impossible de modifier l'article (doublon ou catégorie invalide).");
            }
            return Ok(existant);
        }
    }
}

