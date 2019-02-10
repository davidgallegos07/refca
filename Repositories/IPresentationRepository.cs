using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface IPresentationRepository
    {
        Task<QueryResult<Presentation>> GetPresentations(PresentationQuery filter);
        Task<QueryResult<Presentation>> GetAdminPresentations(PresentationQuery filter);
        Task<QueryResult<Presentation>> GetTeacherPresentations(string userId, PresentationQuery filter);
        Task<int> GetNumberOfPresentationsByTeacher(string userId);

        void Add(Presentation presentation);
        void Remove(Presentation presentation);
        Task<Presentation> GetPresentation(int id);
    }
}