using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Resources;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface IResearchRepository
    {
        Task<QueryResult<Research>> GetResearch(ResearchQuery filter);
        Task<QueryResult<Research>> GetAdminResearch(ResearchQuery filter);
        Task<QueryResult<Research>> GetTeacherResearch(string userId, ResearchQuery filter);
        Task<int> GetNumberOfResearchByTeacher(string userId);
    }
}