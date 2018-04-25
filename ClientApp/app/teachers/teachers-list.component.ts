import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { ITeacher } from '../shared/ITeacher';
import { TeacherService } from '../services/teacher.service';
import { PagerService } from '../services/pager-service';
import { FormControl } from "@angular/forms";


@Component({
	selector: 'teachers',
	templateUrl: './teachers-list.component.html',
	styleUrls: ['./teachers-list.component.css']
})
export class TeachersListComponent {

	public teachers: any = [];
	public pager: any = {};
	query: any = { page: 1 };
	loading: boolean = true;
	hasTeachers: boolean = false;
	constructor(private teacherSvc: TeacherService,
		private pagerSvc: PagerService) { }

	getTeachersList() {
		this.teacherSvc.getTeachersList(this.query)
			.subscribe(t => {
				this.teachers = t,
					this.loading = false;
				this.hasTeachers = t.length < 1 ? true : false;
				this.setPage(this.query.page, this.teachers.totalItems);
			});
	}

	onFilterChange(page?: number) {
		if (page < 1 || (page > this.pager.totalPages) || this.query.page === page)
			return;
		this.query.page = page ? page : this.query.page = 1;
		this.getTeachersList();
	}

	setPage(page: number, totalItems?: number) {

		this.pager = this.pagerSvc.getPager(this.query.page, this.teachers.totalItems);
	}
	ngOnInit() {
		this.getTeachersList();
	}
}