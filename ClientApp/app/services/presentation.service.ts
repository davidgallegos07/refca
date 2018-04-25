import { Injectable } from '@angular/core';
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { IPresentation } from "../shared/IPresentation";

@Injectable()
export class PresentationService {
    private readonly numberOfPresentations: "/api/presentations/count";
    private readonly presentationEndPoint = "/api/presentations";
    constructor(private http: Http) { }


    getPresentationsList(PresentationFilter): Observable<IPresentation[]> {
        return this.http.get(this.presentationEndPoint + '?' + this.toQueryString(PresentationFilter))
            .map(res => res.json());
    }

    getPresentation(id: number): Observable<IPresentation> {
        return this.http.get(`${this.presentationEndPoint}/${id}`)
            .map(res => res.json());
    }

    getPresentationsCount() {
        return this.http.get(this.numberOfPresentations).map(res => res.json());
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
    