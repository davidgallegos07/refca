import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IResearch } from '../shared/IResearch';
import { ResearchService } from '../services/research.service';


@Component({
	selector: 'research',
	templateUrl: './research-details.component.html',
})
export class ResearchDetailsComponent implements OnInit, OnDestroy {
	public rdetails: IResearch;
	private sub: Subscription;

	constructor(private _route: ActivatedRoute,
		private researchSvc: ResearchService,
		private _router: Router) { }

	getResearchDetails(id: number) {
		this.researchSvc.getResearch(id).subscribe(rdetails => this.rdetails = rdetails);
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => {
			let id = +params['id'];
			this.getResearchDetails(id)
		});
	}
	ngOnDestroy() {
		this.sub.unsubscribe();
	}
	onBack(): void {
		this._router.navigate(['/research']);
	}

}