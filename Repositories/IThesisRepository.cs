using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Resources;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface IThesisRepository
    {
        Task<QueryResult<Thesis>> GetTheses(ThesisQuery filter);
        Task<QueryResult<Thesis>> GetAdminTheses(ThesisQuery filter);
        Task<QueryResult<Thesis>> GetTeacherTheses(string userId, ThesisQuery filter);
        Task<int> GetNumberOfThesesByTeacher(string userId);

        void Add(Thesis thesis);
        void Remove(Thesis thesis);
        Task<Thesis> GetTheses(int id);
    }
}