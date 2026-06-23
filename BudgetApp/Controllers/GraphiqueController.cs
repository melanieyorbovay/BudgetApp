using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    public class GraphiqueController : Controller
    {
        public readonly DataContext _context;
        public GraphiqueController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            var totauxParMois = _context.LigneAchats
                 .Include(l => l.IdTicketNavigation)
                 .GroupBy(l => new
                 {
                     Annee = l.IdTicketNavigation.DateAchat.Year,
                     Mois = l.IdTicketNavigation.DateAchat.Month
                 })
                 .Select(g => new
                 {
                     Annee = g.Key.Annee,
                     Mois = g.Key.Mois,
                     Total = g.Sum(l => l.Quantite * l.PrixUnitaire - l.Rabais)
                 })

                 .OrderBy(t => t.Annee)
                 .ThenBy(t => t.Mois)
                 .ToList();

            var labels = totauxParMois
                .Select( t =>$"{new DateOnly(t.Annee, t.Mois, 1):MMM yyyy}")
                .ToList();
            var valeurs = totauxParMois
                .Select(t => t.Total)
                .ToList();
            
            ViewBag.Labels = System.Text.Json.JsonSerializer.Serialize(labels);
            ViewBag.Valeurs = System.Text.Json.JsonSerializer.Serialize(valeurs);

            return View();
        }
    }
}
