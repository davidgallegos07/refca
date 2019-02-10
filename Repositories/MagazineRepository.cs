using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Extensions;
using refca.Resources;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public class MagazineRepository : IMagazineRepository
    {
        private RefcaDbContext context;
        private readonly IMapper mapper;

        private UserManager <ApplicationUser> UserManager;

        public MagazineRepository(RefcaDbContext context, IMapper mapper, UserManager <ApplicationUser> UserManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.UserManager = UserManager;
        }


         public void Add(Magazine magazine)
         {
            context.Magazines.Add(magazine);
        }

         public void Remove(Magazine magazine)
                {
                    context.Magazines.Remove(magazine);
                }

                public  async Task<Magazine> GetMagazines(int id)
                {
                    return await context.Magazines
                    .Include(tp => tp.TeacherMagazines)
                        .ThenInclude(t => t.Teacher)
                    .SingleOrDefaultAsync(a => a.Id ==id);
                }
        public async Task<QueryResult<Magazine>> GetMagazines(MagazineQuery queryObj)
        {
            var result = new QueryResult<Magazine>();
            var query =context.Magazines
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
            var query =context.Magazines
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
            var query =context.Magazines
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
            var magazines = await context.Magazines
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