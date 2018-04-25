import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { IArticle } from '../shared/IArticle';
import { IBook } from '../shared/IBook';
import { IChapterbook } from '../shared/IChapterbook';
import { IMagazine } from '../shared/IMagazine';
import { IPresentation } from '../shared/IPresentation';
import { IResearch } from '../shared/IResearch'
import { ITeacher } from '../shared/ITeacher';
import { IThesis } from '../shared/IThesis';

@Injectable()
export class TeacherService {

	private teachersEndPoint = `/api/teachers`;
	private numberOfTeachers = `/api/Teachers/count`;

	constructor(private http: Http) { }

	getTeachersList(FilterTeacher): Observable<ITeacher[]> {
		return this.http.get(this.teachersEndPoint + `?` + this.toQueryString(FilterTeacher))
			.map((response: Response) => <ITeacher[]>response.json());
	}

	getTeacher(id: string): Observable<ITeacher> {
		return this.http.get(`${this.teachersEndPoint}/${id}`)
			.map(res => res.json());
	}

	getTeachersCount() {
		return this.http.get(this.numberOfTeachers)
			.map(res => res.json());
	}


	getProfileCountArticles(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/articles/count`)
			.map(res => res.json());
	}

	getProfileArticles(id: string, query?): Observable<IArticle[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/articles?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	getProfileCountBooks(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/books/count`)
			.map(res => res.json());
	}

	getProfileBooks(id: string, query?): Observable<IBook[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/books?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	getProfileCountChapterbooks(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/Chapterbooks/count`)
			.map(res => res.json());
	}
	getProfileChapterbooks(id: string, query?): Observable<IChapterbook[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/chapterbooks?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	getProfileCountMagazines(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/Magazines/count`)
			.map(res => res.json());
	}
	getProfileMagazines(id: string, query?): Observable<IMagazine[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/magazines?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	getProfileCountPresentations(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/Presentations/count`)
			.map(res => res.json());
	}
	getProfilePresentations(id: string, query?): Observable<IPresentation[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/presentations?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	getProfileCountResearch(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/Research/count`)
			.map(res => res.json());
	}
	getProfileResearch(id: string, query?): Observable<IResearch[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/research?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	getProfileCountTheses(id: string) {
		return this.http.get(`${this.teachersEndPoint}/${id}/theses/count`)
			.map(res => res.json());
	}
	getProfileTheses(id: string, query?): Observable<IThesis[]> {
		return this.http.get(`${this.teachersEndPoint}/${id}/theses?`+ this.toQueryString(query))
			.map(res => res.json());
	}

	toQueryString(obj) {
		var parts = [];
		for (var property in obj) {
			var value = obj[property];
			if (value != null && value != undefined)
				parts.push(encodeURIComponent(property) + `=` + encodeURIComponent(value));
		}

		return parts.join(`&`);
	}

	private handleError(error: Response) {
		console.error(error);
		return Observable.throw(error.json().error || `Server Error`);
	}

}