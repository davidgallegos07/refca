import { NgModule,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {CommonModule} from '@angular/common';
import { RouterModule } from '@angular/router';

import { ResearchListComponent } from './research-list.component';
import { ResearchDetailsComponent} from './research-details.component';

import {AcademicBodyService} from '../services/academic-body.service';
import { ResearchService } from '../services/research.service';
import { ResearchLineService } from "../services/research-line.service";
import { KnowledgeAreaService } from "../services/knowledge-area.service";
import { SharedModule } from "../shared/shared.module";

@NgModule({
	declarations: [ResearchListComponent, ResearchDetailsComponent],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            {path:'research',component: ResearchListComponent},
            {path:'research/:id',component: ResearchDetailsComponent},
        ])
    ],
	providers: [ResearchService, AcademicBodyService,
    ResearchLineService, KnowledgeAreaService ],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})

export class ResearchModule{}