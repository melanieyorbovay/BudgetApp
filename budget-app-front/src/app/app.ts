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

  constructor() {
    this.articleService.getArticles().subscribe(data => {
      this.articles.set(data);
    });
  }

  articles = signal<Article[]>([]); 

  categories: Categorie[] = [
    { idCategorie: 1, nomCategorie: 'Fruits et legumes' },
    { idCategorie: 2, nomCategorie: 'Boulangerie' },
    { idCategorie: 3, nomCategorie: 'Produits laitiers' },
    { idCategorie: 4, nomCategorie: 'Viande et substituts' },
    { idCategorie: 5, nomCategorie: 'Surgeles' },
    { idCategorie: 6, nomCategorie: 'Conserves' },
    { idCategorie: 7, nomCategorie: 'Pates et riz' },
    { idCategorie: 8, nomCategorie: 'Epices et condiments' },
    { idCategorie: 9, nomCategorie: 'Snacks et confiseries ' },
    { idCategorie: 10, nomCategorie: 'Boissons' },
    { idCategorie: 11, nomCategorie: 'Papeterie' },
    { idCategorie: 12, nomCategorie: 'Autres' },
    { idCategorie: 13, nomCategorie: 'Entretien et nettoyages' }
    
  ];
  unites: string[] = ['piece', 'kg', 'litre', 'boite', 'sachet', 'bouteille', 'carton', 'paquet'];
  
  //ajout de propriétés pour le formulaire d'ajout d'article
  nouveauNomArticle: string = '';
  nouveauUnite: string = 'piece';
  nouveauIdCategorie: number = 0;

  nomCategorie(id: number) {
    const categorie = this.categories.find(c => c.idCategorie === id);
    return categorie ? categorie.nomCategorie : 'Inconnue';
  }

  ajouterArticle() {
    if (this.nouveauNomArticle && this.nouveauUnite && this.nouveauIdCategorie) {
      const nouvelArticle: Article = {
        idArticle: this.articles().length + 1, // Génère un nouvel ID basé sur la longueur du tableau
        nomArticle: this.nouveauNomArticle,
        unite: this.nouveauUnite,
        idCategorie: this.nouveauIdCategorie
      };
      this.articleService.ajouterArticle(nouvelArticle);
      
      // Met à jour la liste des articles
      // Réinitialiser les champs du formulaire
      this.nouveauNomArticle = '';
      this.nouveauUnite = '';
      this.nouveauIdCategorie = 0;
    }
  }

}






