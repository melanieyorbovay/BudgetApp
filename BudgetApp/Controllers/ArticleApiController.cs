using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;

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
    }
}

