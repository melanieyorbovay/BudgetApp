using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    public class TicketController : Controller

    {
        private readonly DataContext _context;

        public TicketController(DataContext context)
        {  _context = context; }
        public IActionResult Index(int? mois, int? annee)
        {
            //avant foreach pour lier ligne (GetAll etc)
            var tickets = _context.Tickets
                                   .Include(t => t.IdMagasinNavigation)
                                   .Include(t => t.LigneAchats)
                                   .AsQueryable();

            if (mois.HasValue && mois > 0)
                tickets = tickets.Where(t => t.DateAchat.Month == mois);

            if (annee.HasValue && annee > 0)
                tickets = tickets.Where(t => t.DateAchat.Year == annee);

            ViewBag.Mois = mois ?? 0;
            ViewBag.Annee = annee ?? 0;

            return View(tickets.OrderByDescending(t => t.DateAchat).ToList());

        }
        public IActionResult Detail(int id)
        {
            var ticket = _context.Tickets
                                   .Include(t => t.IdMagasinNavigation)
                                   .Include(t => t.LigneAchats)
                                        .ThenInclude(l => l.IdArticleNavigation)
                                    .FirstOrDefault(t => t.IdTicket == id);

            if (ticket == null) return NotFound();
            return View(ticket);
        }
        public IActionResult Imprimer(int id)
        {
            var ticket = _context.Tickets
                                 .Include(t => t.IdMagasinNavigation)
                                 .Include(t => t.LigneAchats)
                                        .ThenInclude(l => l.IdArticleNavigation)
                                 .FirstOrDefault(t => t.IdTicket == id);

            if (ticket == null) return NotFound();
            return View(ticket);

        }
        public IActionResult AjouterLignes(int id)
        {
            var ticket = _context.Tickets
                                    .Include(t => t.IdMagasinNavigation)
                                     .Include(t => t.LigneAchats)       
                                    .FirstOrDefault(t => t.IdTicket == id);

            if (ticket == null) return NotFound();

            ViewBag.Articles = _context.Articles
                                        .Include(a => a.IdCategorieNavigation)
                                        .OrderBy(a => a.NomArticle)
                                        .ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(ticket);
        }

        [HttpPost]
        public IActionResult AjouterLigne(int idTicket, int idArticle, decimal quantite, decimal prixUnitaire, decimal rabais)
        { 
            var ligne = new LigneAchat
            {
                IdTicket = idTicket,
                IdArticle = idArticle,
                Quantite = quantite,
                PrixUnitaire = prixUnitaire,
                Rabais = rabais

            };
            _context.LigneAchats.Add(ligne);
            _context.SaveChanges();

            return RedirectToAction("AjouterLignes", new { id = idTicket });
        }

        [HttpPost]
        public IActionResult TerminerTicket(int idTicket)
        {
            return RedirectToAction("Detail", new { id = idTicket });
        }
        public IActionResult Ajouter()
        {
            ViewBag.Magasins = _context.Magasins.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Ajouter(DateOnly dateAchat, int idMagasin)
        {
            var ticket = new Ticket
            {
                DateAchat = dateAchat,
                IdMagasin = idMagasin
            };
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction("AjouterLignes", new { id = ticket.IdTicket });
        }

        [HttpPost]
        public IActionResult AjouterMagasin(string nomMagasin, string localite)
        {
            var magasin = new Magasin
            {
                NomMagasin = nomMagasin.Trim(),
                Localite = localite.Trim()
            };

            _context.Magasins.Add(magasin);
            _context.SaveChanges();

            return RedirectToAction("Ajouter");
        }

        [HttpPost]
        public IActionResult SupprimerLigne(int idAchatLigne, int idTicket)
        {
            var ligne = _context.LigneAchats.Find(idAchatLigne);
            if (ligne != null)
            {
                _context.LigneAchats.Remove(ligne);
                _context.SaveChanges();
            }

            return RedirectToAction("AjouterLignes", new { id = idTicket });
        }

        [HttpPost]
        public IActionResult SupprimerTicket(int idTicket)
        {
            var ticket = _context.Tickets
                                  .Include(t => t.LigneAchats)
                                  .FirstOrDefault(t => t.IdTicket == idTicket);
            if (ticket != null)
            {
                _context.LigneAchats.RemoveRange(ticket.LigneAchats);
                _context.Tickets.Remove(ticket);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AjouterArticle(string nomArticle, string unite, int idCategorie, int idTicket)
        {
           bool existe = _context.Articles
                                  .Any(a => a.NomArticleNormalized == nomArticle.Trim().ToLower());

            //Condition pour le controle de doublons
            if (existe)
            {
                TempData["ErreurArticle"] = $"L'article \"{nomArticle}\" existe déjà. Sélectionner dans la liste.";
                return RedirectToAction("AjouterLignes", new { id = idTicket });
            }

            var nouvel = new Article
            {
                NomArticle = nomArticle.Trim(),
                Unite = unite.Trim(),
                IdCategorie = idCategorie

            };
            _context.Articles.Add(nouvel);
            _context.SaveChanges();

            TempData["SuccesArticle"] = $"Article \"{nomArticle}\" créé avec succès !";
            return RedirectToAction("AjouterLignes", new { id = idTicket });
        }

        public IActionResult ModifierLigne(int id, int idTicket)
        {
            var ligne = _context.LigneAchats
                                .Include(l => l.IdArticleNavigation)
                                .FirstOrDefault(l => l.IdAchatLigne == id);
            if (ligne == null) return NotFound();

            ViewBag.IdTicket = idTicket;
            return View(ligne);
        }

        [HttpPost]
        public IActionResult ModifierLigne(int idAchatLigne, int idTicket, decimal quantite, decimal prixUnitaire, decimal rabais)
        {
            var ligne = _context.LigneAchats.Find(idAchatLigne);

            if (ligne == null) return NotFound();

            ligne.Quantite = quantite;
            ligne.PrixUnitaire = prixUnitaire;
            ligne.Rabais = rabais;

            _context.SaveChanges();

            return RedirectToAction("Detail", new { id = idTicket });
        }
    }
    

}
