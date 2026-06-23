using System;
using System.Collections.Generic;

namespace BudgetApp.ModelsV2;

public partial class Magasin
{
    public int IdMagasin { get; set; }

    public string NomMagasin { get; set; } = null!;

    public string Localite { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
