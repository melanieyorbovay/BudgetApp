using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;
    
    namespace BudgetApp.Controllers
{
    [Route("api/unites")]
    [ApiController]
    public class UniteApiController : ControllerBase
    {
        private readonly DataContext _context;
        public UniteApiController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetUnites()
        {
            var unites = _context.Articles
                                 .Select(a => a.Unite)
                                 .Distinct()
                                 .OrderBy(u => u)
                                 .ToList();
            return Ok(unites);
        }
    }
}
