using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Resources;
using refca.Models;
using refca.Models.QueryFilters;
using refca.Extensions;

namespace refca.Repositories
{
    public class ThesisRepository : IThesisRepository
    {
        private readonly RefcaDbContext _context;
        private readonly IMapper mapper;
        public ThesisRepository(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;
        }

        public async Task<QueryResult<Thesis>> GetTheses(ThesisQuery queryObj)
        {
            var result = new QueryResult<Thesis>();
            var query = _context.Thesis
                 .Include(tp => tp.TeacherTheses)
                   .ThenInclude(t => t.Teacher)
               .Include(e => e.EducationProgram)
               .Include(r => r.ResearchLine)
               .Where(p => p.IsApproved == true)
               .OrderBy(d => d.AddedDate)
               .AsQueryable();

            query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(thesesi => thesesi.TeacherTheses = thesesi.TeacherTheses.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Thesis>> GetAdminTheses(ThesisQuery queryObj)
        {
            var result = new QueryResult<Thesis>();
            var query = _context.Thesis
                 .Include(tp => tp.TeacherTheses)
                   .ThenInclude(t => t.Teacher)
               .Include(e => e.EducationProgram)
               .Include(r => r.ResearchLine)
               .Where(p => p.IsApproved == queryObj.IsApproved)
               .OrderBy(d => d.AddedDate)
               .AsQueryable();


            query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(thesesi => thesesi.TeacherTheses = thesesi.TeacherTheses.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Thesis>> GetTeacherTheses(string userId, ThesisQuery queryObj)
        {
            var result = new QueryResult<Thesis>();
            var query = _context.Thesis
                    .Where(tt => tt.TeacherTheses.Any(t => t.TeacherId == userId))
                .Include(tp => tp.TeacherTheses)
                    .ThenInclude(t => t.Teacher)
               .Include(e => e.EducationProgram)
               .Include(r => r.ResearchLine)
               .Where(p => p.IsApproved == queryObj.IsApproved)
               .OrderBy(d => d.AddedDate)
               .AsQueryable();

            query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(thesesi => thesesi.TeacherTheses = thesesi.TeacherTheses.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<int> GetNumberOfThesesByTeacher(string userId)
        {
            var theses = await _context.Thesis
                .Where(tp => tp.TeacherTheses.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return theses;
        }

        #region helpers
        public static IQueryable<Thesis> ApplyFiltering(IQueryable<Thesis> query, ThesisQuery queryObj)
        {
            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
            {
                var term = queryObj.SearchTerm.ToLower();
                query = query.Where(r =>
                r.Title.ToLower().Contains(term) ||
                r.StudentName.ToLower().Contains(term));
            }

            if (queryObj.EducationProgramId.HasValue)
                query = query.Where(e => e.EducationProgramId == queryObj.EducationProgramId.Value);

            if (queryObj.ResearchLineId.HasValue)
                query = query.Where(r => r.ResearchLineId == queryObj.ResearchLineId.Value);
            return query;
        }
        #endregion
    }
}