using BudgetApp.ModelsV2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; //Include()

namespace BudgetApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly DataContext _context;
        public ArticleController(DataContext context)
        {  _context = context; //remplace new ArticleDAO()
        
        }
        public IActionResult Index(int? idCategorie, string? recherche)
        {

            var articles = _context.Articles
                                    .Include(a => a.IdCategorieNavigation)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(recherche)) //Filtre par recherche
                articles = articles.Where(a => a.NomArticle.Contains(recherche));
            //filtre par catégorie
            if (idCategorie.HasValue && idCategorie > 0)
                articles = articles.Where(a => a.IdCategorie == idCategorie);


                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.IdCategorie = idCategorie ?? 0;
                ViewBag.Recherche = recherche ?? "";

                return View(articles.OrderBy(a => a.NomArticle).ToList());
            }
        public IActionResult Detail(int id) //Tout avec une seule requete Include()
        {
            var article = _context.Articles
                                   .Include(a => a.IdCategorieNavigation)
                                   .Include(a => a.LigneAchats) //Historique
                                   .ThenInclude(l => l.IdTicketNavigation)
                                   .ThenInclude(t => t.IdMagasinNavigation)
                                   .FirstOrDefault(a => a.IdArticle == id);

            if (article == null) return NotFound();

            ViewBag.Lignes = article.LigneAchats
                .OrderBy(l => l.IdTicketNavigation.DateAchat)
                .ToList();

            return View(article);
        }

        [HttpGet]
        public IActionResult Ajouter()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Ajouter(string nomArticle, string unite, int idCategorie)
        {
            bool existe = _context.Articles
                                  .Any(a => a.NomArticleNormalized == nomArticle.Trim().ToLower());
            if (existe)
            {
                ViewBag.Erreur = $"L'article \"{nomArticle}\" existe déjà.";
                ViewBag.Categorie = _context.Categories.ToList();
                return View();
            }

            var nouvel = new Article
            {
                NomArticle = nomArticle.Trim(),
                Unite = unite.Trim(),
                IdCategorie = idCategorie
            };

            _context.Articles.Add(nouvel);
            _context.SaveChanges();
            TempData["Succes"] = $"Article \"{nomArticle}\" ajouté avec succès.";
            return RedirectToAction("Index");
            
        }

        public IActionResult Modifier(int id)
        {
            var article = _context.Articles
                .Include(a => a.IdCategorieNavigation)
                .FirstOrDefault(a => a.IdArticle == id);
            if (article == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(article);
        }
        [HttpPost]
        public IActionResult Modifier(int idArticle, string nomArticle, string unite, int idCategorie)
        {
            var article = _context.Articles.Find(idArticle);
            if (article == null) return NotFound();

            //anti-doublon demande si un autre article contient ce nom
            bool doublon = _context.Articles
                .Any(a => a.IdArticle != idArticle && a.NomArticleNormalized == nomArticle.Trim().ToLower());
            if (doublon)
            {
                ViewBag.Erreur = $"Un autre article \"{nomArticle}\" existe déjà.";
                ViewBag.Categories = _context.Categories.ToList();
                return View(article);
            }

            article.NomArticle = nomArticle.Trim();
            article.Unite = unite.Trim();
            article.IdCategorie = idCategorie;
            _context.SaveChanges();

            TempData["Succes"] = $"Articles \"{article.NomArticle}\" modifié.";
            return RedirectToAction("Index");
        }

        //supression
        [HttpPost]
        public IActionResult Supprimer(int id)
        {
            var article = _context.Articles
                .Include(a => a.LigneAchats)
                .FirstOrDefault(a => a.IdArticle == id);

            if (article == null) return NotFound();

            //il ne faut pas supprimer un article déjà utilisé dans des tickets pour pas perdre l historique
            if (article.LigneAchats.Any())
            {
                TempData["Erreur"] = $"Impossible de supprimer \"{article.NomArticle}\" : il est utilisé dans {article.LigneAchats.Count} ligne(s) d'achat.";
                return RedirectToAction("Index");
            }

            _context.Articles.Remove(article);
            _context.SaveChanges();
            TempData["Succes"] = $"Articles \"{article.NomArticle}\" supprimé.";
            return RedirectToAction("Index");
        }

        //Fusionner des articles si deja doublon en gardant historique:
        public IActionResult Fusionner()
        {
            ViewBag.Articles = _context.Articles
                .Include(a => a.LigneAchats) //affiche le nbr d achats
                .OrderBy(a => a.NomArticle)
                .ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Fusionner(int idGarder, int idFusionner)
        {
            if (idGarder == idFusionner)
            {
                TempData["Erreur"] = "Sélectionner deux articles différents.";
                return RedirectToAction("Fusionner");
            }

            var garder = _context.Articles.Find(idGarder);
            var fusionner = _context.Articles.Find(idFusionner);
            if (garder == null || fusionner == null) return NotFound();
            
            //chaque ligne du doublon va pointer vers l'article gardé
            var lignes = _context.LigneAchats
                .Where(l => l.IdArticle == idFusionner)
                .ToList();
            foreach (var ligne in lignes)
                ligne.IdArticle = idGarder;
            _context.SaveChanges();

            //Suppression sure car le doublon n'a plus de lignes
            _context.Articles.Remove(fusionner);
            _context.SaveChanges();

            TempData["Succes"] = $"Article \"{fusionner.NomArticle}\" fusionné dans \"{garder.NomArticle}\".";
            return RedirectToAction("Index");
        }
    }
 }
