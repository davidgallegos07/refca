import { Component } from '@angular/core';
import { ArticleService } from '../../services/article.service';
import { ComponentStillLoadingError } from '@angular/core/src/linker/compiler';
import { BookService } from '../../services/book.service';
import { ChapterbookService } from '../../services/chapterbook.service';
import { MagazineService } from '../../services/magazine.service';
import { PresentationService } from '../../services/presentation.service';
import { ResearchService } from '../../services/research.service';
import { TeacherService } from '../../services/teacher.service';
import { ThesesService } from '../../services/theses.service';

@Component({
    selector: 'home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.css']
})
export class HomeComponent {
	public articles: any = [];
	query: any = { searchTerm: ""};
	loading: boolean = false;
	hasArticles: boolean = false;
	

	constructor(private articleSvc: ArticleService,private bookSvc: BookService, private chapterbookSvc: ChapterbookService,
	private magazineSvc: MagazineService, private presentationSvc: PresentationService,private researchSvc: ResearchService,
	private teacherSvc: TeacherService, private thesesSvc: ThesesService) { }

	getArticlesList() {
		this.articleSvc.getArticlesList(this.query)
			.subscribe(a => {
				this.articles = a,
				this.loading = true;	
				this.hasArticles = this.articles.totalItems > 0 ? true : false;
			});
	}


	public books: any = [];
	hasBooks: boolean = false;

	getBooksList() {
		this.bookSvc.getBooksList(this.query)
			.subscribe(b => {
				this.books = b,
				this.loading = true;
				this.hasBooks = this.books.totalItems > 0 ? true : false;
			});
		}
	
		public chapterbooks: any = [];
		hasChapterbooks: boolean = false;
	

		getChapterbooksList() {
			this.chapterbookSvc.getChapterbookList(this.query)
				.subscribe(c => {
					this.chapterbooks = c,
					this.loading = true;
					this.hasChapterbooks = this.chapterbooks.totalItems > 0 ? true : false;
				});
		}

		public magazines: any = [];

		hasMagazines: boolean = false;
		
	
		getMagazinesList(){
			this.magazineSvc.getMagazinesList(this.query)
				.subscribe(m => {
					this.magazines = m,
					this.loading = true;
					this.hasMagazines = this.magazines.totalItems > 0 ? true : false;
				}); 
		}

		public presentations: any = [];
		hasPresentations: boolean = false;
	
	
		getPresentationsList(){
			this.presentationSvc.getPresentationsList(this.query)
			.subscribe(p => {
				this.presentations = p,
				this.loading = true;
				this.hasPresentations = this.presentations.totalItems > 0 ? true : false;
			}); 
		}
		
	
		public research: any = [];
		hasResearch: boolean = false;
	
		
	
		getResearchList() {
			this.researchSvc.getResearchList(this.query)
				.subscribe(r => {
					this.research = r,
					this.loading = true;
					this.hasResearch = this.research.totalItems > 0 ? true : false;
				});
		}

		public teachers: any = [];
		hasTeachers: boolean = false;


		getTeachersList() {
			this.teacherSvc.getTeachersList(this.query)
				.subscribe(t => {
					this.teachers = t,
					this.loading = true;
					this.hasTeachers = t.length > 0 ? true : false;
				});
		}


		public theses: any = [];
		hasTheses: boolean = false;

		getThesesList() {
			this.thesesSvc.getThesesList(this.query)
				.subscribe(te => { 
					this.theses = te,
					this.loading = true;
					this.hasTheses = this.theses.totalItems > 0 ? true : false;
				});
		}

		restoreToFalse(){
			this.hasArticles = false;
			this.hasBooks = false;
			this.hasMagazines = false;
			this.hasChapterbooks = false;
			this.hasResearch = false;
			this.hasTheses = false;
			this.hasPresentations = false;
			this.hasResearch = false;
		}


		getAll(){
			this.restoreToFalse();
			if(this.query.searchTerm != ""){
				this.getArticlesList();
				this.getBooksList();
				this.getChapterbooksList();
				this.getMagazinesList();
				this.getPresentationsList();
				this.getResearchList();
				this.getThesesList()
			}
		}
			
}
