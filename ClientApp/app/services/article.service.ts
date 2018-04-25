import { Injectable } from '@angular/core';
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { IArticle } from "../shared/IArticle";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { AppError } from '../common/app-error';
import { NotFoundError } from "../common/not-found-error";
import { Router } from '@angular/router';
@Injectable()
export class ArticleService {

    private readonly articleEndPoint = "/api/articles";
    private readonly numberOfArticles = "/api/articles/count";
    constructor(private http: Http,
        private router: Router) { }

    getArticlesList(articleFilter): Observable<IArticle[]> {
        return this.http.get(this.articleEndPoint + '?' + this.toQueryString(articleFilter))
            .map(res => res.json());
    }

    getArticle(id: number): Observable<IArticle> {
        return this.http.get(`${this.articleEndPoint}/${id}`)
            .map(res => res.json());
            //.catch(this.handleError);
    }

    getArticlesCount() {
        return this.http.get(this.numberOfArticles).map(res => res.json());
    }

    toQueryString(obj) {
        var parts = [];
        for (var property in obj) {
            var value = obj[property];
            if (value != null && value != undefined)
                parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
        }

        return parts.join('&');
    }
    private handleError(error: any) {
        // if (error.status === 404){
        //     return Observable.throw(new NotFoundError(error));
        // }
        console.log('atleastbby');
        //this.router.navigate(['/articles']);
        return Observable.of({description: "Error Value Emitted"});
    }
}