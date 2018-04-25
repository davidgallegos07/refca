import {Http} from '@angular/http';
import {IResearchLine} from '../shared/IResearchLine';
import { Injectable } from "@angular/core";
import {Observable} from 'rxjs/Observable';

@Injectable()
export class ResearchLineService{
    private readonly ResearchLineEndPoint = "/api/researchlines"
    constructor(private http: Http){}

    getAllResearchLines(): Observable<IResearchLine[]>{
        return this.http.get(this.ResearchLineEndPoint)
            .map(res => res.json())
    }
}