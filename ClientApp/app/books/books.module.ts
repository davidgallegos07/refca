import { NgModule,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {CommonModule} from '@angular/common';
import { RouterModule } from '@angular/router';


import { BooksListComponent } from './books-list.component';
import { BooksDetailsComponent } from './books-details.component';
import { BookService } from '../services/book.service';
import { SharedModule } from "../shared/shared.module";

@NgModule({
	declarations: [BooksListComponent, BooksDetailsComponent],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        SharedModule,
        CommonModule,
        RouterModule.forChild([
            {path:'books',component: BooksListComponent},
            {path:'books/:id',component: BooksDetailsComponent},
        ])
    ],
	providers: [BookService],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})

export class BooksModule{}