using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Extensions;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public class PresentationRepository : IPresentationRepository
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;

        public PresentationRepository(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
        }


        public async Task<QueryResult<Presentation>> GetPresentations(PresentationQuery queryObj)
        {
            var result = new QueryResult<Presentation>();
            var query = _context.Presentations
                .Include(tp => tp.TeacherPresentations)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();
            
            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(presentation => presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Presentation>> GetAdminPresentations(PresentationQuery queryObj)
        {
            var result = new QueryResult<Presentation>();
            var query = _context.Presentations
                .Include(tp => tp.TeacherPresentations)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();
            
            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(presentation => presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Presentation>> GetTeacherPresentations(string userId, PresentationQuery queryObj)
        {
            var result = new QueryResult<Presentation>();
            var query = _context.Presentations
                .Where(tp => tp.TeacherPresentations.Any(t => t.TeacherId == userId))
                .Include(tp => tp.TeacherPresentations)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();
            
            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(presentation => presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }
        
        public async Task<int> GetNumberOfPresentationsByTeacher(string userId)
        {
             var presentations = await _context.Presentations
                .Where(tp => tp.TeacherPresentations.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();
            
            return presentations;
        }

        #region helpers
        public static IQueryable<Presentation> ApplyFiltering(IQueryable<Presentation> query, PresentationQuery queryObj)
        {
            var term = queryObj.SearchTerm.ToLower();
            query = query.Where(p => 
            p.Title.ToLower().Contains(term) ||
            p.Congress.ToLower().Contains(term));
            return query;
        }
        #endregion
    }
}