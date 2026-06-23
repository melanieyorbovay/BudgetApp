using System;
using System.Collections.Generic;

namespace BudgetApp.ModelsV2;

public partial class LigneAchat
{
    public int IdAchatLigne { get; set; }

    public int IdTicket { get; set; }

    public int IdArticle { get; set; }

    public decimal Quantite { get; set; }

    public decimal PrixUnitaire { get; set; }

    public decimal Rabais { get; set; }

    public virtual Article IdArticleNavigation { get; set; } = null!;

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
