export interface IMagazine {
	id: number;
	title: string;
	index: string;
	issn: number;
	editor: string;
	edition: number;
	editionDate: string;
	isApproved: boolean;
	magazinePath: string;
	teacherMagazines: any;
}