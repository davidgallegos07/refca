import { NgModule,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ArticlesListComponent } from './articles-list.component';
import { ArticlesDetailsComponent } from './articles-details.component';
import { PageNotFoundComponent } from '../page-not-found/page-not-found.component';

import { ArticleService } from '../services/article.service';
import { PagerService } from '../services/pager-service';
import { SharedModule } from "../shared/shared.module";
import { ToastNotificationService } from "../services/toast-notification.service";
import { ToastyModule } from "ng2-toasty";


@NgModule({
    declarations:[ArticlesListComponent,ArticlesDetailsComponent],
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        ToastyModule.forRoot(),
        SharedModule,
        RouterModule.forChild([
            {path:'articles',component: ArticlesListComponent},
            {path:'articles/:id',component: ArticlesDetailsComponent},
        ])
    ],
    providers:[ArticleService, PagerService],
    schemas:[CUSTOM_ELEMENTS_SCHEMA]
})

export class ArticlesModule{}