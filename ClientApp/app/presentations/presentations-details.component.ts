import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IPresentation } from '../shared/IPresentation';
import { PresentationService } from '../services/presentation.service';


@Component({
    selector: 'presentations',
    templateUrl: './presentations-details.component.html',
})
export class PresentationsDetailsComponent implements OnInit, OnDestroy {
	public pdetails: IPresentation;
	private sub: Subscription;

	constructor(private _route: ActivatedRoute, private presentationSvc: PresentationService, private _router: Router) { }

	getPresentationsDetails(id: number) {
		this.presentationSvc.getPresentation(id).subscribe(pdetails => this.pdetails = pdetails);
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => { let id = +params['id']; this.getPresentationsDetails(id) });
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/presentations']);
	}
	
}
