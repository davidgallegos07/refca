import { Injectable } from '@angular/core';
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { IBook } from "../shared/IBook";

@Injectable()
export class BookService {
    private readonly numberOfBooks: "/api/books/count";
    private readonly bookEndPoint = "/api/books";
    constructor(private http: Http) { }

    getBooksList(BookFilter): Observable<IBook[]> {
        return this.http.get(this.bookEndPoint + '?' + this.toQueryString(BookFilter))
            .map(res => res.json());
    }

    getBook(id: number): Observable<IBook> {
        return this.http.get(`${this.bookEndPoint}/${id}`)
            .map(res => res.json());
    }

    getBooksCount() {
        return this.http.get(this.numberOfBooks).map(res => res.json());
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