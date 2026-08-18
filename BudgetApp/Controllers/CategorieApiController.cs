using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategorieApiController : ControllerBase
    {
        private readonly DataContext _context;
        public CategorieApiController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories
                .OrderBy(c => c.IdCategorie)
                .ToList();
            return Ok(categories);
        }
    }
}
