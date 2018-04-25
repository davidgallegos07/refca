import { NgModule, CUSTOM_ELEMENTS_SCHEMA, ErrorHandler } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';

import { AppComponent } from './components/app/app.component';

import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FootersComponent } from './components/footer/footer.component';
import { HelpComponent } from './components/help/help.component';
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";

import { ArticlesModule } from './articles/articles.module';
import { BooksModule } from './books/books.module';
import { ChapterBooksModule } from './chapterbooks/chapterbooks.module';
import { MagazinesModule } from './magazines/magazines.module';
import { PresentationsModule } from './presentations/presentations.module';
import { ResearchModule } from './research/research.module';
import { TeachersModule } from './teachers/teachers.module';
import { ThesesModule } from './theses/theses.module';

import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounce'
import 'rxjs/add/operator/debounceTime'
import 'rxjs/Rx';
import { AppErrorHandler } from "./app.error-handler";
import { SharedModule } from "./shared/shared.module";
import { ToastyModule } from "ng2-toasty";

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
		NavMenuComponent,
		FootersComponent,
		HomeComponent,
		HelpComponent,
		PageNotFoundComponent
    ],
    imports: [
        UniversalModule, 
		ToastyModule.forRoot(),
		//SharedModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
			{ path: 'home', component: HomeComponent },
			{ path: 'help', component: HelpComponent },
			{ path: '**', component: PageNotFoundComponent}
		]),
		ArticlesModule,
		BooksModule,
		ChapterBooksModule,
		MagazinesModule,
		PresentationsModule,
		ResearchModule,
		FormsModule,
		ReactiveFormsModule,
		TeachersModule,
		ThesesModule
	],
	providers: [
		{ provide: ErrorHandler, useClass: AppErrorHandler },
	],
	schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule {
}
