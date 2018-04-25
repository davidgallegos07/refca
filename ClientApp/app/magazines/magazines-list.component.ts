import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { IMagazine } from '../shared/IMagazine';
import { MagazineService } from '../services/magazine.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";

@Component({
    selector: 'magazines',
    templateUrl: './magazines-list.component.html',
})
export class MagazinesListComponent {

	public magazines: any = [];
	public pager: any = {};
	query: any = {page: 1};
	loading: boolean = true;
	hasMagazines: boolean = false;
	
	constructor(private magazineSvc: MagazineService, private pagerSvc: PagerService) { }

	getMagazinesList(){
		this.magazineSvc.getMagazinesList(this.query)
			.subscribe(m => {
				this.magazines = m,
				this.loading = false;
				this.hasMagazines = this.magazines.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.magazines.totalItems);
			}); 
	}
	
	ngOnInit() { 
		this.getMagazinesList();	
	}

	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;			
		this.getMagazinesList();
	}

	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.magazines.totalItems);
	}

}