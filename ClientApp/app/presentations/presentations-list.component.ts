import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { IPresentation } from '../shared/IPresentation';
import { PresentationService } from '../services/presentation.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";

@Component({
    selector: 'presentations',
    templateUrl: './presentations-list.component.html',
})
export class PresentationsListComponent {

	public presentations: any = [];
	public pager: any = {};
	query: any = {page: 1};
	loading: boolean = true;
	hasPresentations: boolean = false;

	constructor(private presentationSvc: PresentationService, private pagerSvc: PagerService) { }

	getPresentationsList(){
		this.presentationSvc.getPresentationsList(this.query)
		.subscribe(p => {
				this.presentations = p,
				this.loading = false;
				this.hasPresentations = this.presentations.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.presentations.totalItems);
			}); 
	}

	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;			
		this.getPresentationsList();
	}

	ngOnInit() { 
		this.getPresentationsList();
	}

	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.presentations.totalItems);
	}

}