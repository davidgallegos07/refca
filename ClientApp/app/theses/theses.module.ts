import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ThesesListComponent } from './theses-list.component';
import { ThesisDetailsComponent } from './thesis-details.component';
import { TeacherService } from '../services/teacher.service';

import { EducationProgramService } from '../services/education-program.service'
import { ResearchLineService } from "../services/research-line.service";
import { ThesesService } from "../services/theses.service";
import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [ThesesListComponent, ThesisDetailsComponent],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            {
                path: 'theses', 
                component: ThesesListComponent,
            },
                        {
                path: 'theses/:id', 
                component: ThesisDetailsComponent,
            }
        ])
    ],
    providers: [EducationProgramService, ResearchLineService,
        ThesesService],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ThesesModule { }