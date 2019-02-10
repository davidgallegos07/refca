using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface IMagazineRepository
    {
        Task<QueryResult<Magazine>> GetMagazines(MagazineQuery filter);
        Task<QueryResult<Magazine>> GetAdminMagazines(MagazineQuery filter);
        Task<QueryResult<Magazine>> GetTeacherMagazines(string userId, MagazineQuery filter);
        Task<int> GetNumberOfMagazinesByTeacher(string userId);

        void Add(Magazine magazine);

        void Remove(Magazine magazine);

        Task<Magazine> GetMagazines(int id);
    
    }
}