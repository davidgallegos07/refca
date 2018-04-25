import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { TeachersListComponent } from './teachers-list.component';
import { TeachersDetailsComponent } from './teachers-details.component';
import { TeacherArticlesComponent } from './teacher-articles/teacher-articles.component';
import { TeacherThesesComponent } from './teacher-theses/teacher-theses.component';
import { TeacherResearchComponent } from './teacher-research/teacher-research.component';

import { TeacherService } from '../services/teacher.service';

import { SharedModule } from '../shared/shared.module';
import { TeacherMagazinesComponent } from "./teacher-magazines/teacher-magazines.component";
import { TeacherBooksComponent } from "./teacher-books/teacher-books.component";
import { TeacherPresentationsComponent } from "./teacher-presentations/teacher-presentations.component";
import { TeacherChapterbooksComponent } from "./teacher-chapterbooks/teacher-chapterbooks.component";

@NgModule({
    declarations: [
        TeachersListComponent,
        TeachersDetailsComponent,
        TeacherArticlesComponent,
        TeacherBooksComponent,
        TeacherChapterbooksComponent,
        TeacherThesesComponent,
        TeacherPresentationsComponent,
        TeacherResearchComponent,
        TeacherMagazinesComponent
    ],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: 'teachers', component: TeachersListComponent },
            {
                path: 'teachers/:id',
                component: TeachersDetailsComponent,
                children: [
                    {
                        path: '',
                        component: TeacherArticlesComponent
                    },
                    {
                        path: 'theses',
                        component: TeacherThesesComponent
                    },
                    {
                        path: 'articles',
                        component: TeacherArticlesComponent
                    },
                    {
                        path: 'books',
                        component: TeacherBooksComponent
                    },
                    {
                        path: 'chapterbooks',
                        component: TeacherChapterbooksComponent
                    },
                    {
                        path: 'presentations',
                        component: TeacherPresentationsComponent
                    },
                    {
                        path: 'magazines',
                        component: TeacherMagazinesComponent
                    },
                    {
                        path: 'research',
                        component: TeacherResearchComponent
                    },
                ]
            },
        ])
    ],
    providers: [TeacherService],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class TeachersModule { }