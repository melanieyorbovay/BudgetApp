using BudgetApp.ModelsV2;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var tickets = _context.Tickets
                                .Include(t => t.IdMagasinNavigation)
                                .Include(t => t.LigneAchats)
                                .ThenInclude(l => l.IdArticleNavigation)
                                .ToList();

            ViewBag.TotalDepenses = tickets.Sum(t => t.Total).ToString("N2");
            ViewBag.NbTickets = tickets.Count;
            ViewBag.NbArticles = _context.Articles.Count();

            ViewBag.DerniersTickets = tickets
                .OrderByDescending(t => t.DateAchat)
                .Take(10)
                .ToList();

            return View();
        }
    

    }
}
