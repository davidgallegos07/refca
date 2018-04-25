export interface IArticle {
	id: number;
	title: string;
	magazine: string;
	issn: number;
	editionDate: string;
	isApproved: boolean;
	articlePath: string;
	teacherArticles: any;
	totalItems: number;
}