using Microsoft.EntityFrameworkCore;

namespace BudgetApp.ModelsV2
{
    public static class SeedData
    {
        public static void Initialize(DataContext context)
        {
            //Si des catégorie existent on ne fait rien, sinon a chaque redemarrage on va recréer les données de base
            if (context.Categories.Any())
                return;
            var alimentation = new Categorie { NomCategorie = "Alimentation" };
            var boissons = new Categorie { NomCategorie = "Boissons" };
            var menage = new Categorie { NomCategorie = "Ménage" };
            var hygiene = new Categorie { NomCategorie = "Hygiène" };

            context.Categories.AddRange(alimentation, boissons, menage, hygiene);
            // SaveChanges ici pour que SQL Server attribue les IdCategorie.
            // On utilise ensuite la propriété de navigation, donc EF Core
            // résoudrait la FK tout seul — mais sauvegarder en deux temps
            // rend l'ordre explicite et le code plus lisible.
            context.SaveChanges();

            context.Articles.AddRange(
                new Article { NomArticle = "Pain paysan", Unite = "pièce", IdCategorieNavigation = alimentation },
                new Article { NomArticle = "Lait entier", Unite = "litre", IdCategorieNavigation = alimentation },
                new Article { NomArticle = "Pommes Gala", Unite = "kg", IdCategorieNavigation = alimentation },
                new Article { NomArticle = "Coca", Unite = "litre", IdCategorieNavigation = boissons },
                new Article { NomArticle = "Lessive", Unite = "pièce", IdCategorieNavigation = menage },
                new Article { NomArticle = "Savon", Unite = "pièce", IdCategorieNavigation = hygiene }
            );
            context.SaveChanges();
        }
    }
}