import {Http} from '@angular/http';
import {IKnowledgeArea} from '../shared/IKnowledgeArea';
import { Injectable } from "@angular/core";
import {Observable} from 'rxjs/Observable';

@Injectable()
export class KnowledgeAreaService{
    public knowledgeArea: IKnowledgeArea[];
    private readonly KnowledgeAreasEndPoint = "/api/knowledgeareas"
    constructor(private http: Http){}

    getAllKnowledgeAreas(): Observable<IKnowledgeArea[]>{
        return this.http.get(this.KnowledgeAreasEndPoint)
            .map(res => res.json())
    }
}