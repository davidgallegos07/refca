using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface IChapterbookRepository
    {
        Task<QueryResult<Chapterbook>> GetChapterbooks(ChapterbookQuery filter);
        Task<QueryResult<Chapterbook>> GetAdminChapterbooks(ChapterbookQuery filter);
        Task<QueryResult<Chapterbook>> GetTeacherChapterbooks(string userId, ChapterbookQuery filter);
        Task<int> GetNumberOfChapterbooksByTeacher(string userId);
        void Add(Chapterbook chapterbook);
        void Remove(Chapterbook chapterbook);
        Task<Chapterbook> GetChapterbook(int id);
    }
}