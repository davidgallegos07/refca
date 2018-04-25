import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IMagazine } from '../shared/IMagazine';
import { MagazineService } from '../services/magazine.service';


@Component({
    selector: 'magazines',
    templateUrl: './magazines-details.component.html',
})
export class MagazinesDetailsComponent {
	public mdetails: IMagazine;
	private sub: Subscription;
	constructor(private _route: ActivatedRoute, private magazineSvc: MagazineService, private _router: Router) { }

	getMagazinesDetails(id: number) {
		this.magazineSvc.getMagazine(id).subscribe(mdetails => this.mdetails = mdetails);
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => { let id = +params['id']; this.getMagazinesDetails(id) });
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/magazines']);
	}
}
