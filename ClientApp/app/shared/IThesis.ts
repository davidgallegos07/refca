export interface IThesis {
	id: number;
	title: string;
	studentName: string;
	publishedDate: string;
	thesisPath: string;
	isApproved: boolean;
	educationProgram: any[];
	researchLine: any;
	teacherTheses: any;
}