import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IBook } from '../shared/IBook';
import { BookService } from '../services/book.service';

@Component({
    selector: 'books',
    templateUrl: './books-details.component.html',
})
export class BooksDetailsComponent implements OnInit, OnDestroy {

	public bdetails: IBook;
	private sub: Subscription;
	constructor(private _route: ActivatedRoute, private bookSvc: BookService, private _router: Router) { }

	getBooksDetails(id: number) {
		this.bookSvc.getBook(id).subscribe(bdetails => this.bdetails = bdetails);
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => { let id = +params['id'];
		this.getBooksDetails(id)
		 });
	}
	
	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/books']);
	}
}