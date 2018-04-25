import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IArticle } from '../shared/IArticle';
import { ArticleService } from '../services/article.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";

@Component({
	selector: 'articles',
	templateUrl: './articles-list.component.html',
})

export class ArticlesListComponent implements OnInit {
	
	public articles: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasArticles: boolean = false;

	constructor(private articleSvc: ArticleService, private pagerSvc: PagerService) { }

	getArticlesList() {
		this.articleSvc.getArticlesList(this.query)
			.subscribe(a => {
				this.articles = a,
				this.loading = false;
				this.hasArticles = this.articles.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.articles.totalItems);
			});
	}

	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getArticlesList();
	}

	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.articles.totalItems);
	}

	ngOnInit() {
		this.getArticlesList();
	}

}