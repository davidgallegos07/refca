import { Component, OnInit, OnDestroy } from '@angular/core';
import { TeacherService } from '../../services/teacher.service'
import { IResearch } from "../../shared/IResearch";
import { ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { PagerService } from "../../services/pager-service";
import { FormControl } from "@angular/forms";

@Component({
    selector: 'teacher-research',
    templateUrl: './teacher-research.component.html'
})
export class TeacherResearchComponent implements OnInit, OnDestroy {
    
    public research: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasResearch: boolean = false;
    userId: string;
    private sub: Subscription;

    constructor(private teacherSvc: TeacherService,
        private _route: ActivatedRoute, private _router: Router,
        private pagerSvc: PagerService) { }

    	getResearchList(id: string, query) {
		this.teacherSvc.getProfileResearch(id, query)
			.subscribe(a => {
				this.research = a,
				this.loading = false;
				this.hasResearch = a.length < 1 ? true : false;
				this.setPage(this.query.page, this.research.totalItems);
			});
	}

    onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getResearchList(this.userId, this.query);
	}

    ngOnInit(): void {
        this.sub = this._route.parent.params.subscribe(params => {
            let id = params['id'];
            this.userId = id;
            this.getResearchList(this.userId, this.query);
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    onBack(): void {
        this._router.navigate(['/teachers']);
    }

    setPage(page: number, totalItems?: number) {

        this.pager = this.pagerSvc.getPager(this.query.page, this.research.totalItems);
    }
}