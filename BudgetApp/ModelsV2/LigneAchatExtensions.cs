namespace BudgetApp.ModelsV2
{
    public partial class LigneAchat
    {
        public Article? Article => IdArticleNavigation;
        public decimal MontantLigne => Quantite * PrixUnitaire - Rabais;

    }
}
