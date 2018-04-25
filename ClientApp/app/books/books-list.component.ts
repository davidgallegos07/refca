import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IBook } from '../shared/IBook';
import { TeacherService } from '../services/teacher.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";
import { BookService } from "../services/book.service";

@Component({
	selector: 'books',
	templateUrl: './books-list.component.html',
})

export class BooksListComponent {

	public books: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasBooks: boolean = false;

	constructor(private bookSvc: BookService, private pagerSvc: PagerService) { }
	
	getBooksList() {
		this.bookSvc.getBooksList(this.query)
			.subscribe(b => {
				this.books = b,
				this.loading = false;
				this.hasBooks = this.books.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.books.totalItems);
			});
	}

	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getBooksList();
	}

	ngOnInit() {
		this.getBooksList();
	}

	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.books.totalItems);
	}

}