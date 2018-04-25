import { Injectable } from '@angular/core';
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { IMagazine } from "../shared/IMagazine";

@Injectable()
export class MagazineService {
    private readonly numberOfMagazines: "/api/magazines/count";
    private readonly magazineEndPoint = "/api/magazines";
    constructor(private http: Http) { }

    getMagazinesList(MagazineFilter): Observable<IMagazine[]> {
        return this.http.get(this.magazineEndPoint + '?' + this.toQueryString(MagazineFilter))
            .map(res => res.json());
    }

    getMagazine(id: number): Observable<IMagazine> {
        return this.http.get(`${this.magazineEndPoint}/${id}`)
            .map(res => res.json());
    }

    getMagazinesCount() {
        return this.http.get(this.numberOfMagazines).map(res => res.json());
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