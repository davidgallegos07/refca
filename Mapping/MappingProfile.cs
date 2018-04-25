using System.Linq;
using AutoMapper;
using refca.Resources;
using refca.Resources.TeacherResources;
using refca.Models;
using refca.Models.QueryFilters;
using refca.Resources.QueryResources;
using refca.Resources.TeacherQueryResources;
using refca.Models.ArticleViewModels;
using refca.Models.BookViewModels;

namespace refca.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleViewModel>().ReverseMap();
            CreateMap<Book, BookViewModel>().ReverseMap();
                            
            CreateMap<Teacher, TeacherResource>();
            CreateMap<Thesis, ThesisResource>();
            CreateMap<Research, ResearchResource>();
            CreateMap<Book, BookResource>();
            CreateMap<Chapterbook, ChapterbookResource>();
            CreateMap<Article, ArticleResource>();
            CreateMap<Magazine, MagazineResource>();
            CreateMap<EducationProgram, EducationProgramResource>();
            CreateMap<ResearchLine, ResearchLineResource>();
            CreateMap<AcademicBody, AcademicBodyResource>();
            CreateMap<KnowledgeArea, KnowledgeAreaResource>();
            CreateMap<ConsolidationGrade, ConsolidationGradeResource>();
            
            CreateMap<Thesis, TeacherThesisResource>();
            CreateMap<Research, TeacherResearchResource>();
            CreateMap<Book, TeacherBookResource>();
            CreateMap<Chapterbook, TeacherChapterbookResource>();
            CreateMap<Article, TeacherArticleResource>();
            CreateMap<Magazine, TeacherMagazineResource>();
            
            CreateMap<ResearchQueryResource, ResearchQuery>();
            CreateMap<ArticleQueryResource, ArticleQuery>();
            CreateMap<PresentationQueryResource, PresentationQuery>();
            CreateMap<ThesisQueryResource, ThesisQuery>();
            CreateMap<BookQueryResource, BookQuery>();
            CreateMap<MagazineQueryResource, MagazineQuery>();
            CreateMap<ChapterbookQueryResource, ChapterbookQuery>();
            CreateMap<TeacherQueryResource, TeacherQuery>();
            
            CreateMap<TeacherArticleQueryResource, ArticleQuery>();
            CreateMap<TeacherPresentationQueryResource, PresentationQuery>();
            CreateMap<TeacherThesisQueryResource, ThesisQuery>();
            CreateMap<TeacherBookQueryResource, BookQuery>();
            CreateMap<TeacherMagazineQueryResource, MagazineQuery>();
            CreateMap<TeacherChapterbookQueryResource, ChapterbookQuery>();
            CreateMap<TeacherResearchQueryResource, ResearchQuery>();


            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            CreateMap<Teacher, TeacherProductivityResource>();
            CreateMap<Teacher, SimpleTeacherResource>();

            CreateMap<Thesis, ThesisResource>()
                .ForMember(dto => dto.TeacherTheses, opt => opt.MapFrom(s => s.TeacherTheses.Select(t => t.Teacher)));
            CreateMap<Research, ResearchResource>()
                .ForMember(dto => dto.TeacherResearch, opt => opt.MapFrom(s => s.TeacherResearch.Select(t => t.Teacher)));
            CreateMap<Chapterbook, ChapterbookResource>()
                .ForMember(dto => dto.TeacherChapterbooks, opt => opt.MapFrom(s => s.TeacherChapterbooks.Select(t => t.Teacher)));
            CreateMap<Book, BookResource>()
                .ForMember(dto => dto.TeacherBooks, opt => opt.MapFrom(s => s.TeacherBooks.Select(t => t.Teacher)));
            CreateMap<Article, ArticleResource>()
                .ForMember(dto => dto.TeacherArticles, opt => opt.MapFrom(s => s.TeacherArticles.Select(t => t.Teacher)));
            CreateMap<Presentation, PresentationResource>()
                .ForMember(dto => dto.TeacherPresentations, opt => opt.MapFrom(s => s.TeacherPresentations.Select(t => t.Teacher)));
            CreateMap<Magazine, MagazineResource>()
                .ForMember(dto => dto.TeacherMagazines, opt => opt.MapFrom(s => s.TeacherMagazines.Select(t => t.Teacher)));

             CreateMap<Thesis, TeacherThesisResource>()
                .ForMember(dto => dto.TeacherTheses, opt => opt.MapFrom(s => s.TeacherTheses.Select(t => t.Teacher)));
            CreateMap<Research, TeacherResearchResource>()
                .ForMember(dto => dto.TeacherResearch, opt => opt.MapFrom(s => s.TeacherResearch.Select(t => t.Teacher)));
            CreateMap<Chapterbook, TeacherChapterbookResource>()
                .ForMember(dto => dto.TeacherChapterbooks, opt => opt.MapFrom(s => s.TeacherChapterbooks.Select(t => t.Teacher)));
            CreateMap<Book, TeacherBookResource>()
                .ForMember(dto => dto.TeacherBooks, opt => opt.MapFrom(s => s.TeacherBooks.Select(t => t.Teacher)));
            CreateMap<Article, TeacherArticleResource>()    
                .ForMember(dto => dto.TeacherArticles, opt => opt.MapFrom(s => s.TeacherArticles.Select(t => t.Teacher)))
            .ForMember(dto => dto.Role, opt => opt.MapFrom(r => r.TeacherArticles.Where(i => i.TeacherId == i.Article.Owner).Select(x => x.Role).First()));
            CreateMap<Presentation, TeacherPresentationResource>()
                .ForMember(dto => dto.TeacherPresentations, opt => opt.MapFrom(s => s.TeacherPresentations.Select(t => t.Teacher)));
            CreateMap<Magazine, TeacherMagazineResource>()
                .ForMember(dto => dto.TeacherMagazines, opt => opt.MapFrom(s => s.TeacherMagazines.Select(t => t.Teacher)));

        }
    }
}