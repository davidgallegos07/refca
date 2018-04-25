import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IResearch } from '../shared/IResearch';
import { ResearchService } from '../services/research.service';
import { PagerService } from '../services/pager-service';
import { IAcademicBody } from "../shared/IAcademicBody";
import { IResearchLine } from "../shared/IResearchLine";
import { IKnowledgeArea } from "../shared/IKnowledgeArea";
import { FormControl } from "@angular/forms";

@Component({
	selector: 'research',
	templateUrl: './research-list.component.html',
})
export class ResearchListComponent {

	public academicBodies: IAcademicBody[];
	public researchLines: IResearchLine[];
	public knowledgeAreas: IKnowledgeArea[];
	query: any = {page: 1};
	public research: any = [];
	public pager: any = {};
	loading: boolean = true;
	hasResearch: boolean = false;

	constructor(private researchSvc: ResearchService, private pagerSvc: PagerService,) { }

	getResearchList() {
		this.researchSvc.getResearchList(this.query)
			.subscribe(r => {
				this.research = r,
					this.loading = false;
				this.hasResearch = this.research.totalItems < 1 ? true : false;
				this.setPage(this.query.page, this.research.totalItems);
			});
	}

	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;			
		this.getResearchList();
	}

	ngOnInit() {
		this.getAllAcademicBodies();
		this.getAllResearchLines();
		this.getAllKnowledgeAreas();
		this.getResearchList();
	}

	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.research.totalItems);
	}

	getAllAcademicBodies() {
		this.researchSvc.getAllAcademicBodies()
			.subscribe(academicBodies => this.academicBodies = academicBodies)
	}
	getAllResearchLines() {
		this.researchSvc.getAllResearchLines()
			.subscribe(researchLines => this.researchLines = researchLines)
	}
	getAllKnowledgeAreas() {
		this.researchSvc.getAllKnowledgeAreas()
			.subscribe(knowledgeAreas => this.knowledgeAreas = knowledgeAreas)
	}

}