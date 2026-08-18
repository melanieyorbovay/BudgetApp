import { Injectable, inject } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article} from './article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7166/api/articles';

    getArticles(): Observable<Article[]> {
        return this.http.get<Article[]>(this.apiUrl);
    }
    ajouterArticle(article: Article): void {
        
    }
}

