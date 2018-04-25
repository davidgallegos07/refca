import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IThesis} from '../shared/IThesis';

import { ThesesService } from '../services/theses.service';


@Component({
    selector: 'theses',
    templateUrl: './thesis-details.component.html',
})
export class ThesisDetailsComponent implements OnInit,OnDestroy {
	public thesis: IThesis[];
	private sub: Subscription;

	constructor(private _route: ActivatedRoute, private thesesSvc: ThesesService, private _router: Router) { }

	getThesisDetails(id:number) {
		this.thesesSvc.getThesis(id).subscribe(thesis => this.thesis = thesis);
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => { let id = +params['id'];
		this.getThesisDetails(id)		
	 });
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/theses']);
	}
    
}
