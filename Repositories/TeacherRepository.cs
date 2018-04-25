using refca.Data;
using refca.Resources;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using refca.Models;
using Microsoft.EntityFrameworkCore;
using refca.Extensions;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private IHostingEnvironment _environment;
        private RefcaDbContext _context;

        public TeacherRepository(IHostingEnvironment environment, RefcaDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        public async Task<QueryResult<Teacher>> GetTeachers(TeacherQuery queryObj)
        {
            var result = new QueryResult<Teacher>();
            var query = _context.Teachers.AsQueryable();
            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
            {
                var term = queryObj.SearchTerm.ToLower().Trim();
                query = query.Where(t =>
                t.Name.ToLower().Contains(term) ||
                t.Email.ToLower().Contains(term));
            }

            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging(queryObj);
            result.Items = await query.ToListAsync();

            return result;
        }
    }
}