import { Component, signal, inject } from '@angular/core';
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
  messageErreur: string = '';

  constructor() {
    this.articleService.getArticles().subscribe(data => {
      this.articles.set(data);
    });
    this.articleService.getCategories().subscribe(data => {
      this.categories.set(data);
    });
    this.articleService.getUnites().subscribe(data => {
      this.unites.set(data);
    });
  }

  nomCategorie(id: number) {
    const categorie = this.categories().find(c => c.idCategorie === id);
    return categorie ? categorie.nomCategorie : 'Inconnue';
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
          this.messageErreur = '';
        },
        error: (err) => {
          this.messageErreur = err.status === 409
          ? err.error
          : 'Erreur lors de l\'ajout.';
        }
      });
      
    }
  }

}






