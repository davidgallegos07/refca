import { Injectable } from '@angular/core';
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { IChapterbook } from "../shared/IChapterbook";

@Injectable()
export class ChapterbookService {

    private readonly chapterbookEndPoint = "/api/chapterbooks";
    private readonly numberOfChapterbooks = "/api/chapterbooks/count";
    constructor(private http: Http) { }


	getChapterbookList(articleFilter): Observable<IChapterbook[]> {
		return this.http.get(this.chapterbookEndPoint + '?' +this.toQueryString(articleFilter))
        .map(res => res.json());
	}

	getChapterbook(id: number): Observable<IChapterbook> {
		return this.http.get(`${this.chapterbookEndPoint}/${id}`)
          .map(res => res.json());
	}

	getChapterbooksCount() {
			return this.http.get(this.numberOfChapterbooks).map(res => res.json());
	}

    toQueryString(obj) {
        var parts = [];
        for (var property in obj) {
            var value = obj[property];
            if (value != null && value != undefined)
                parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
        }

        return parts.join('&');
    }
}