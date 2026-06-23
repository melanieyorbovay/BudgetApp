using System;
using System.Collections.Generic;

namespace BudgetApp.ModelsV2;

public partial class Categorie
{
    public int IdCategorie { get; set; }

    public string NomCategorie { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
