import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ChapterBooksListComponent } from './chapterbooks-list.component';
import { ChapterBooksDetailsComponent } from './chapterbooks-details.component';
import { ChapterbookService } from '../services/chapterbook.service';

import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [
        ChapterBooksListComponent,
        ChapterBooksDetailsComponent,
    ],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'chapterbooks', component: ChapterBooksListComponent },
            { path: 'chapterbooks/:id', component: ChapterBooksDetailsComponent },
        ])
    ],
    providers: [ChapterbookService],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class ChapterBooksModule { }