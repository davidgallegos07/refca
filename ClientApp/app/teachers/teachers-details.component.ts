import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

import { ITeacher } from '../shared/ITeacher';

import { TeacherService } from '../services/teacher.service';


@Component({
	selector: 'teachers',
	templateUrl: './teachers-details.component.html',
	styleUrls: ['./teachers-details.component.css']
})
export class TeachersDetailsComponent implements OnInit, OnDestroy {

	public teacher: ITeacher;
	public thesesCount = [];
	public articlesCount = [];
	public booksCount = [];
	public chbooksCount = [];
	public magazinesCount = [];
	public presentationsCount = [];
	public researchCount = [];

	public particles = [];
	public pbooks = [];
	public pchbooks = [];
	public pmagazines = [];
	public ppresentations = [];
	public presearch = [];
	public ptheses = [];
	loading: boolean = true;
	private sub: Subscription;
	constructor(private _route: ActivatedRoute, private teacherSvc: TeacherService, private _router: Router) { }

	getTeachersDetails(id: string) {
		this.teacherSvc.getTeacher(id)
			.subscribe(teacher => {
			this.teacher = teacher;
			this.loading = false;
			});
	}

	ngOnInit(): void {
		this.sub = this._route.params.subscribe(params => {
			let id = params['id'];
			this.getTeachersDetails(id);
			this.getProfileCountTheses(id);
			this.getProfileCountArticles(id);
			this.getProfileCountBooks(id);
			this.getProfileCountChapterbooks(id);
			this.getProfileCountMagazines(id);
			this.getProfileCountResearch(id);
			this.getProfileCountPresentations(id);
			this.getProfileArticles(id);
			this.getProfileBooks(id);
			this.getProfileChapterbooks(id);
			this.getProfileMagazines(id);
			this.getProfilePresentations(id);
			this.getProfileResearch(id);
			this.getProfileTheses(id);

		});
	}

	ngOnDestroy() {
		this.sub.unsubscribe();
	}

	onBack(): void {
		this._router.navigate(['/teachers']);
	}

	getProfileCountTheses(id: string) {
		this.teacherSvc.getProfileCountTheses(id)
			.subscribe(thesesCount => this.thesesCount = thesesCount);
	}

	getProfileCountArticles(id: string) {
		this.teacherSvc.getProfileCountArticles(id)
			.subscribe(articlesCount => this.articlesCount = articlesCount);
	}

	getProfileCountBooks(id: string) {
		this.teacherSvc.getProfileCountBooks(id)
			.subscribe(booksCount => this.booksCount = booksCount);
	}

	getProfileCountChapterbooks(id: string) {
		this.teacherSvc.getProfileCountChapterbooks(id)
			.subscribe(chbooksCount => this.chbooksCount = chbooksCount);
	}

	getProfileCountMagazines(id: string) {
		this.teacherSvc.getProfileCountMagazines(id)
			.subscribe(magazinesCount => this.magazinesCount = magazinesCount);
	}

	getProfileCountPresentations(id: string) {
		this.teacherSvc.getProfileCountPresentations(id)
			.subscribe(presentationsCount => this.presentationsCount = presentationsCount);
	}

	getProfileCountResearch(id: string) {
		this.teacherSvc.getProfileCountResearch(id)
			.subscribe(researchCount => this.researchCount = researchCount);
	}


	getProfileArticles(id: string) {
		this.teacherSvc.getProfileArticles(id)
			.subscribe(particles => this.particles = particles);
	}

	getProfileBooks(id: string) {
		this.teacherSvc.getProfileBooks(id)
			.subscribe(pbooks => this.pbooks = pbooks);
	}

	getProfileChapterbooks(id: string) {
		this.teacherSvc.getProfileChapterbooks(id)
			.subscribe(pchbooks => this.pchbooks = pchbooks);
	}

	getProfileMagazines(id: string) {
		this.teacherSvc.getProfileMagazines(id)
			.subscribe(pmagazines => this.pmagazines = pmagazines);
	}

	getProfilePresentations(id: string) {
		this.teacherSvc.getProfilePresentations(id)
			.subscribe(ppresentations => this.ppresentations = ppresentations);
	}

	getProfileResearch(id: string) {
		this.teacherSvc.getProfileResearch(id)
			.subscribe(presearch => this.presearch = presearch);
	}

	getProfileTheses(id: string) {
		this.teacherSvc.getProfileTheses(id)
			.subscribe(ptheses => this.ptheses = ptheses);
	}
}
