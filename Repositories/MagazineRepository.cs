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
    public class MagazineRepository : IMagazineRepository
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;

        public MagazineRepository(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
        }

        public async Task<QueryResult<Magazine>> GetMagazines(MagazineQuery queryObj)
        {
            var result = new QueryResult<Magazine>();
            var query = _context.Magazines
              .Include(tp => tp.TeacherMagazines)
                  .ThenInclude(t => t.Teacher)
              .Where(p => p.IsApproved == true)
              .OrderBy(d => d.AddedDate)
              .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Magazine>> GetAdminMagazines(MagazineQuery queryObj)
        {
            var result = new QueryResult<Magazine>();
            var query = _context.Magazines
              .Include(tp => tp.TeacherMagazines)
                  .ThenInclude(t => t.Teacher)
              .Where(p => p.IsApproved == queryObj.IsApproved)
              .OrderBy(d => d.AddedDate)
              .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Magazine>> GetTeacherMagazines(string userId, MagazineQuery queryObj)
        {
            var result = new QueryResult<Magazine>();
            var query = _context.Magazines
                .Where(tb => tb.TeacherMagazines.Any(t => t.TeacherId == userId))
                .Include(tp => tp.TeacherMagazines)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }
        
        public async Task<int> GetNumberOfMagazinesByTeacher(string userId)
        {
            var magazines = await _context.Magazines
                .Where(tp => tp.TeacherMagazines.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();
            
            return magazines;
        }

        #region helpers
        public static IQueryable<Magazine> ApplyFiltering(IQueryable<Magazine> query, MagazineQuery queryObj)
        {
            var term = queryObj.SearchTerm.ToLower();
            query = query.Where(p =>
            p.Title.ToLower().Contains(term) ||
            p.Editor.ToLower().Contains(term) ||
            Convert.ToString(p.ISSN).ToLower().Contains(term) ||
            p.Index.ToLower().Contains(term));
            return query;
        }
        #endregion
    }
}