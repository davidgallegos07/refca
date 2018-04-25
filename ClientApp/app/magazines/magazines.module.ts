
import { NgModule,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {CommonModule} from '@angular/common';
import { RouterModule } from '@angular/router';

import { MagazinesListComponent } from './magazines-list.component';
import { MagazinesDetailsComponent } from './magazines-details.component';


import { MagazineService } from '../services/magazine.service';
import { SharedModule } from "../shared/shared.module";

@NgModule({
	declarations: [MagazinesListComponent, MagazinesDetailsComponent],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            {path:'magazines',component: MagazinesListComponent},
            {path:'magazines/:id',component:MagazinesDetailsComponent},
        ])
    ],
	providers: [MagazineService],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})

export class MagazinesModule{}