namespace BudgetApp.ModelsV2
{
    public partial class Article
    {
        public Categorie? Categorie => IdCategorieNavigation;
    }
}
