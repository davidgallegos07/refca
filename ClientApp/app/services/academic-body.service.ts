import {Http} from '@angular/http';
import {IAcademicBody} from '../shared/IAcademicBody';
import { Injectable } from "@angular/core";
import {Observable} from 'rxjs/Observable';


@Injectable()
export class AcademicBodyService{
    public academicBodies: IAcademicBody[];
    private readonly AcademicBodyEndPoint = "/api/academicbodies"
    constructor(private http: Http){}

    getAllAcademicBodies():Observable<IAcademicBody[]>{
        return this.http.get(this.AcademicBodyEndPoint)
            .map(res => res.json())
    }
}