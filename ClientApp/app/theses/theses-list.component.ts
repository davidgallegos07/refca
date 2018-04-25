import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx';

import { IThesis } from '../shared/IThesis';
import { EducationProgramService } from '../services/education-program.service';
import { ResearchLineService } from '../services/research-line.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";
import { ThesesService } from "../services/theses.service";
import { IResearchLine } from "../shared/IResearchLine";
import { IEducationProgram } from "../shared/IEducationProgram";

@Component({
	selector: 'theses',
	templateUrl: './theses-list.component.html',
})
export class ThesesListComponent {

	public theses: any = [];
	public pager: any = {};
	public educationPrograms: IEducationProgram[];
	public researchLines: IResearchLine[];
	query: any = {page: 1};
	loading: boolean = true;
	hasTheses: boolean = false;

	constructor(private educationProgramSvc: EducationProgramService,
		private researchLineSvc: ResearchLineService,
		private thesesSvc: ThesesService,
		private pagerSvc: PagerService) { }

	getThesesList() {
		this.thesesSvc.getThesesList(this.query)
			.subscribe(t => { 
				this.theses = t,
				this.loading = false;
				this.hasTheses = this.theses.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.theses.totalItems);
			});
	}

	getAllEducationPrograms() {
		this.educationProgramSvc.getEducationPrograms()
			.subscribe(educationPrograms => this.educationPrograms = educationPrograms)
	}

	getAllResearchLines() {
		this.researchLineSvc.getAllResearchLines()
			.subscribe(researchLines => this.researchLines = researchLines)
	}
	
	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;			
		this.getThesesList();
	}
	
	ngOnInit() {
		this.getAllResearchLines();
		this.getAllEducationPrograms();
		this.getThesesList();		
	}

	setPage(page: number, totalItems?: number) {
		
		this.pager = this.pagerSvc.getPager(this.query.page, this.theses.totalItems);
	}
}