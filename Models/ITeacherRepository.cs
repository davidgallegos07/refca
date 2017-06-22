using refca.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public interface ITeacherRepository
    {
        void TeacherThesis(IEnumerable<Thesis> theses, string id);
        void TeacherResearch(IEnumerable<Research> research, string id);
        void TeacherPresentations(IEnumerable<Presentation> presentations, string id);
        void TeacherChapterbooks(IEnumerable<Chapterbook> chapterbooks, string id);
        void TeacherBooks(IEnumerable<Book> books, string id);
        void TeacherArticles(IEnumerable<Article> articles, string id);
        void TeacherMagazines(IEnumerable<Magazine> magazines, string id);
    }
}