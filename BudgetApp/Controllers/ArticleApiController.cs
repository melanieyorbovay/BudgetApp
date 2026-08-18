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
    }
}

