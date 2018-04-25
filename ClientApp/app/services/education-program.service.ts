import { Http } from '@angular/http';
import { IEducationProgram} from '../shared/IEducationProgram';
import { Observable } from 'rxjs/Observable';
import { Injectable } from "@angular/core";

@Injectable()
export class EducationProgramService{
    private readonly educationProgramEndPoint = "/api/educationprograms";
    constructor(private http: Http){}

    getEducationPrograms(): Observable<IEducationProgram[]>{
        return this.http.get(this.educationProgramEndPoint)
            .map(res => res.json());
    }

}