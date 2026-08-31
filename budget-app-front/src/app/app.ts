import { Component, computed, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Article } from './article.model';
import { Categorie }  from './categorie.model';
import { ArticleService } from './article.service';

//decorateur:
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule], //sinon ngModel oas reconnu (équivent using)
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('budget-app-front');
  
  private articleService = inject(ArticleService);

  articles = signal<Article[]>([]);
  categories = signal<Categorie[]>([]);
  unites = signal<string[]>([]);

  //Proptiétés pour le formulaire d'ajout d'articles
  nouveauNomArticle: string = '';
  nouveauUnite: string = 'pièce';
  nouveauIdCategorie: number = 0;
  //signal : modifié dans un callback asynchrone, doit donc déclencher le rendu
  messageErreur = signal<string>('');

  //Filtres
  protected readonly termRecherche = signal<string>('');
  protected readonly categorieFiltre = signal<string>('');

  /** Index idCategorie -> nom. L'équivalent d'un Dictionary<int, string>
   *  recalculé seulement quand catégorie() change.*/
private readonly indexCategories = computed(
  () => new Map(this.categories().map(c => [c.idCategorie, c.nomCategorie]))
);

/**Le pendant client du .Where(...) de l'Index() MVC.*/
protected readonly articlesFiltres = computed(() => {
  const terme = this.termRecherche().trim().toLowerCase();
  const cat = this.categorieFiltre();

  return this.articles().filter(a => {
    const okTerme = terme === ''
     || (a.nomArticle ?? '').toLowerCase().includes(terme);
    const okCategorie = cat === ''
      || String(a.idCategorie) === cat;
    return okTerme && okCategorie;
  });
});

  constructor() {
    this.articleService.getArticles().subscribe({
       next: (data) => this.articles.set(data),
       error: () => this.messageErreur.set('Impossible de charger les articles.')
    });
    this.articleService.getCategories().subscribe({
      next: (data) => this.categories.set(data),
      error: () => this.messageErreur.set('Impossible de charger les catégories.')
    });
    this.articleService.getUnites().subscribe({
      next: (data) => this.unites.set(data),
      error: () => this.messageErreur.set('Impossible de charger les unités.')
    });
  }

    // Handlers de filtres
    protected majRecherche(event: Event): void {
      this.termRecherche.set((event.target as HTMLInputElement).value);
    }
    protected majCategorie(event: Event): void {
      this.categorieFiltre.set((event.target as HTMLSelectElement).value);
    }
    protected reinitialiserFiltres(): void {
      this.termRecherche.set('');
      this.categorieFiltre.set('');
    }
    protected nomCategorie(id: number | null | undefined): string {
      return (id != null ? this.indexCategories().get(id) : undefined) ?? '-';
    }

  ajouterArticle() {
    if (this.nouveauNomArticle && this.nouveauUnite && this.nouveauIdCategorie) {
      const nouvelArticle: Article = {
        //idArticle: this.articles().length + 1, // Génère un nouvel ID basé sur la longueur du tableau
        idArticle: 0, // L'Api l'ignore, la base génère le vrai
        nomArticle: this.nouveauNomArticle,
        unite: this.nouveauUnite,
        idCategorie: this.nouveauIdCategorie
      };
      this.articleService.ajouterArticle(nouvelArticle).subscribe({
        next: (articleCree) => {
          this.articles.update(liste => [...liste, articleCree]);
          this.nouveauNomArticle = '';
          this.nouveauUnite = 'pièce';
          this.nouveauIdCategorie = 0;
          this.messageErreur.set('');
        },
        error: (err) => {
          this.messageErreur.set(

          err.status === 409 ? err.error : 'Erreur lors de l\'ajout.'
          );
      }
    });
      
    }
  }

}






