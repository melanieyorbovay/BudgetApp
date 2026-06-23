using System;
using System.Collections.Generic;

namespace BudgetApp.ModelsV2;

public partial class Ticket
{
    public int IdTicket { get; set; }

    public DateOnly DateAchat { get; set; }

    public int IdMagasin { get; set; }

    public virtual Magasin IdMagasinNavigation { get; set; } = null!;

    public virtual ICollection<LigneAchat> LigneAchats { get; set; } = new List<LigneAchat>();
}
