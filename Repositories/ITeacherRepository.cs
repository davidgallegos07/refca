using refca.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface ITeacherRepository
    {
        Task<QueryResult<Teacher>> GetTeachers(TeacherQuery filter);
    }
}