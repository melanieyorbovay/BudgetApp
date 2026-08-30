import { Injectable, inject } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article} from './article.model';
import { Categorie } from './categorie.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
    private http = inject(HttpClient);

    private apiUrlArticles = 'https://localhost:7166/api/articles';
    private apiUrlCategories = 'https://localhost:7166/api/categories';
    private apiUrlUnites = 'https://localhost:7166/api/unites';

    getArticles(): Observable<Article[]> {
        return this.http.get<Article[]>(this.apiUrlArticles);
    }

    getCategories(): Observable<Categorie[]> {
        return this.http.get<Categorie[]>(this.apiUrlCategories);
    }

    getUnites(): Observable<string[]> {
        return this.http.get<string[]>(this.apiUrlUnites);
    }

    ajouterArticle(article: Article): Observable<Article> {
        return this.http.post<Article>(this.apiUrlArticles, article);
    }
}

