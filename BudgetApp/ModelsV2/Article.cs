using System;
using System.Collections.Generic;

namespace BudgetApp.ModelsV2;

public partial class Article
{
    public int IdArticle { get; set; } //Detection de PK par convention ("Id-Nom")

    public string NomArticle { get; set; } = null!; //null indique que EF Core remplira toujours cette valeur

    public string Unite { get; set; } = null!;

    public int IdCategorie { get; set; } //EF détecte que c'est une FK vers Categorie

    public string? NomArticleNormalized { get; set; }
    //acces categorie liée sans JOIN 
    public virtual Categorie IdCategorieNavigation { get; set; } = null!;
    //Tous les LignesAchat de cet article (navinverse)
    public virtual ICollection<LigneAchat> LigneAchats { get; set; } = new List<LigneAchat>();
}
