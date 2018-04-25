import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { IChapterbook } from '../shared/IChapterbook';
import { ChapterbookService } from '../services/chapterbook.service';


@Component({
    selector: 'chapterbooks',
    templateUrl: './chapterbooks-details.component.html',
})
export class ChapterBooksDetailsComponent implements OnInit, OnDestroy {

	public cbdetails: IChapterbook;
	private sub: Subscription;

	constructor(private _route: ActivatedRoute, private chapterbookSvc: ChapterbookService, private _router: Router) { }

	getChapterBooksDetails(id: number) {
		this.chapterbookSvc.getChapterbook(id).subscribe(cbdetails => this.cbdetails = cbdetails);
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => { let id = +params['id']; this.getChapterBooksDetails(id) });
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/chapterbooks']);
	}
}
