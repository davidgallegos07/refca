import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IChapterbook } from '../shared/IChapterbook';
import { ChapterbookService } from '../services/chapterbook.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";

@Component({
	selector: 'chapterbooks',
	templateUrl: './chapterbooks-list.component.html',
})
export class ChapterBooksListComponent {
	public chapterbooks: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasChapterbooks: boolean = false;

	constructor(private chapterbookSvc: ChapterbookService, private pagerSvc: PagerService) { }

	getChapterbooksList() {
		this.chapterbookSvc.getChapterbookList(this.query)
			.subscribe(c => {
				this.chapterbooks = c,
					this.loading = false;
				this.hasChapterbooks = this.chapterbooks.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.chapterbooks.totalItems);
			});
	}

	ngOnInit() {
		this.getChapterbooksList();
	}
	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getChapterbooksList();
	}
	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.chapterbooks.totalItems);
	}
}