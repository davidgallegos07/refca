import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IArticle } from '../shared/IArticle';
import { ArticleService } from '../services/article.service';
import { AppError } from "../common/app-error";
import { NotFoundError } from "../common/not-found-error";

@Component({
	selector: 'articles',
	templateUrl: './articles-details.component.html',
})
export class ArticlesDetailsComponent implements OnInit, OnDestroy {
	public article: IArticle;
	private sub: Subscription;
	constructor(private _route: ActivatedRoute,
		private articleSvc: ArticleService,
		private _router: Router,
	) { }

	getArticlesDetails(id: number) {
		this.articleSvc.getArticle(id).
			subscribe(article => {
				this.article = article
			},
			(error) => {
				if (error instanceof NotFoundError)
					console.log('This is a not found error managed');

					//this._router.navigate(['/articles']);					
			// 	}
			
			// 	return Observable.of(null);
			console.error('psoible failure');
			}
		);
	}

	ngOnInit(): void {
		this.sub = this._route.params
			.subscribe(params => {
				let id = +params['id'];

				if (isNaN(id) || id < 0)
					return this._router.navigate(['/articles']);

				this.getArticlesDetails(id);
			});
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/articles']);
	}
}