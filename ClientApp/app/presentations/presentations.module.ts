import { NgModule,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import {CommonModule} from '@angular/common';
import { RouterModule } from '@angular/router';

import { PresentationsListComponent} from './presentations-list.component';
import { PresentationsDetailsComponent } from './presentations-details.component';

import { PresentationService } from '../services/presentation.service';
import { SharedModule } from "../shared/shared.module";

@NgModule({
	declarations: [PresentationsListComponent, PresentationsDetailsComponent],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            {path:'presentations',component:PresentationsListComponent},
            {path:'presentations/:id',component:PresentationsDetailsComponent},
        ])
    ],
	providers: [PresentationService],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})

export class PresentationsModule{}