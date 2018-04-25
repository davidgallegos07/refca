import { Component, OnInit, OnDestroy } from '@angular/core';
import { TeacherService } from '../../services/teacher.service'
import { IBook } from "../../shared/IBook";
import { ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { PagerService } from "../../services/pager-service";

@Component({
    selector: 'teacher-books',
    templateUrl: './teacher-books.component.html'
})
export class TeacherBooksComponent implements OnInit, OnDestroy{
    
    public books: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasBooks: boolean = false;
    userId: string;
    private sub: Subscription;

    constructor(private teacherSvc: TeacherService,
        private _route: ActivatedRoute, private _router: Router,
        private pagerSvc: PagerService) { }

    	getBooksList(id: string, query) {
		this.teacherSvc.getProfileBooks(id, query)
			.subscribe(a => {
				this.books = a,
				this.loading = false;
				this.hasBooks = a.length < 1 ? true : false;
				this.setPage(this.query.page, this.books.totalItems);
			});
	}

    onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getBooksList(this.userId, this.query);
	}

    ngOnInit(): void {
        this.sub = this._route.parent.params.subscribe(params => {
            let id = params['id'];
            this.userId = id;
            this.getBooksList(this.userId, this.query);
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    onBack(): void {
        this._router.navigate(['/teachers']);
    }

    setPage(page: number, totalItems?: number) {

        this.pager = this.pagerSvc.getPager(this.query.page, this.books.totalItems);
    }
}