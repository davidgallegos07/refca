import { Http } from '@angular/http';
import { AcademicBodyService } from "./academic-body.service";
import { ResearchLineService } from "./research-line.service";
import { KnowledgeAreaService } from "./knowledge-area.service";
import { IResearch } from '../shared/IResearch';
import { Injectable } from "@angular/core";
import { Observable } from 'rxjs/Observable';
import { IResearchLine } from "../shared/IResearchLine";


@Injectable()
export class ResearchService {

	private readonly researchEndPoint = "/api/research";
	private readonly numberOfResearch = "/api/research/count";

	constructor(
		private academicBodyService: AcademicBodyService,
		private researchLineService: ResearchLineService,
		private knowledgeAreaService: KnowledgeAreaService,
		private http: Http) { }

	getResearchList(ResearchQuery): Observable<IResearch[]> {
		return this.http.get(this.researchEndPoint + '?' + this.toQueryString(ResearchQuery))
			.map(res => res.json());
	}
	
    getSearch(ResearchQuery): Observable<IResearchLine[]>{
       return this.http.get(this.researchEndPoint + '?' + this.toQueryString(ResearchQuery))
			.map(res => res.json());
    }
	
	getResearch(id: number): Observable<IResearch> {
		return this.http.get(`${this.researchEndPoint}/${id}`)
			.map(res => res.json());
	}

	toQueryString(obj) {
		var parts = [];
		for (var property in obj) {
			var value = obj[property];
			if (value != null && value != undefined)
				parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
		}

		return parts.join('&');
	}
	
	getResearchCount() {
		return this.http.get(this.numberOfResearch).map(res => res.json());
	}

	getAllAcademicBodies() {
		return this.academicBodyService.getAllAcademicBodies();
	}

	getAllResearchLines(){
		return this.researchLineService.getAllResearchLines();
	}
	getAllKnowledgeAreas(){
		return this.knowledgeAreaService.getAllKnowledgeAreas();
	}

}