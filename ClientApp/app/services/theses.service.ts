import { Injectable } from '@angular/core';
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { IThesis } from "../shared/IThesis";

@Injectable()
export class ThesesService {
    private readonly thesisEndPoint = "/api/theses";
    private readonly numberOfTheses = "/api/theses/count";
    
    constructor(private http: Http) { }

    getThesesList(FilterTheses): Observable<IThesis[]>{
        return this.http.get(this.thesisEndPoint + '?' + this.toQueryString(FilterTheses))
            .map(res => res.json());
    }

    getThesis(id: number): Observable<IThesis[]> {
        return this.http.get(`${this.thesisEndPoint}/${id}`)
             	.map((response: Response) => <IThesis[]>response.json());
             
    }

    getThesesCount() {
        return this.http.get(this.numberOfTheses).map(res => res.json());
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