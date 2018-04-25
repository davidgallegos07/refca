import { Component, OnInit, OnDestroy } from '@angular/core';
import { TeacherService } from '../../services/teacher.service'
import { IMagazine } from "../../shared/IMagazine";
import { ActivatedRoute, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { PagerService } from "../../services/pager-service";

@Component({
    selector: 'teacher-magazines',
    templateUrl: './teacher-magazines.component.html'
})
export class TeacherMagazinesComponent implements OnInit, OnDestroy{
    
    public magazines: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasMagazines: boolean = false;
    userId: string;
    private sub: Subscription;

    constructor(private teacherSvc: TeacherService,
        private _route: ActivatedRoute, private _router: Router,
        private pagerSvc: PagerService) { }

    	getMagazinesList(id: string, query) {
		this.teacherSvc.getProfileMagazines(id, query)
			.subscribe(a => {
				this.magazines = a,
				this.loading = false;
				this.hasMagazines = a.length < 1 ? true : false;
				this.setPage(this.query.page, this.magazines.totalItems);
			});
	}

    onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getMagazinesList(this.userId, this.query);
	}

    ngOnInit(): void {
        this.sub = this._route.parent.params.subscribe(params => {
            let id = params['id'];
            this.userId = id;
            this.getMagazinesList(this.userId, this.query);
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    onBack(): void {
        this._router.navigate(['/teachers']);
    }

    setPage(page: number, totalItems?: number) {

        this.pager = this.pagerSvc.getPager(this.query.page, this.magazines.totalItems);
    }
}