using System.Linq;

namespace BudgetApp.ModelsV2
{
    //partial complete la classe généré par Efcore
    public partial class Ticket
    {
        //alias pour garder la compatibilité avec les vues
        public Magasin? Magasin => IdMagasinNavigation;
        public IEnumerable<LigneAchat> Lignes => LigneAchats;
        //Propriété calculé n existe plus on recalcule direct:
        public decimal Total => LigneAchats
            .Sum(l => l.Quantite * l.PrixUnitaire - l.Rabais);

    }
}
