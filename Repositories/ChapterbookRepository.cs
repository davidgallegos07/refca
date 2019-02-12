using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Extensions;
using refca.Models;
using refca.Models.QueryFilters;
using refca.Resources;
using Microsoft.AspNetCore.Http;


namespace refca.Repositories
{
    public class ChapterbookRepository : IChapterbookRepository
    {
        private RefcaDbContext context;
        private readonly IMapper mapper;

        private UserManager<ApplicationUser> userManager;

        public ChapterbookRepository(RefcaDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Chapterbook> GetChapterbook(int id)
        {
            return await context.Chapterbooks
                  .Include(tp => tp.TeacherChapterbooks)
                      .ThenInclude(t => t.Teacher)
                  .SingleOrDefaultAsync(a => a.Id == id);

        }
        public void Add(Chapterbook chapterbook)
        {
            context.Chapterbooks.Add(chapterbook);
        }
        public void Remove(Chapterbook chapterbook)
        {
            context.Remove(chapterbook);
        }

        public async Task<QueryResult<Chapterbook>> GetChapterbooks(ChapterbookQuery queryObj)
        {
            var result = new QueryResult<Chapterbook>();
            var query = context.Chapterbooks
                 .Include(tp => tp.TeacherChapterbooks)
                     .ThenInclude(t => t.Teacher)
                 .Where(a => a.IsApproved == true)
                 .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(article => article.TeacherChapterbooks = article.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();
            return result;
        }

        public async Task<QueryResult<Chapterbook>> GetAdminChapterbooks(ChapterbookQuery queryObj)
        {
            var result = new QueryResult<Chapterbook>();
            var query = context.Chapterbooks
                .Include(tp => tp.TeacherChapterbooks)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Chapterbook>> GetTeacherChapterbooks(string userId, ChapterbookQuery queryObj)
        {
            var result = new QueryResult<Chapterbook>();
            var query = context.Chapterbooks
                .Where(tc => tc.TeacherChapterbooks.Any(t => t.TeacherId == userId))
                .Include(tp => tp.TeacherChapterbooks)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<int> GetNumberOfChapterbooksByTeacher(string userId)
        {
            var chapterbooks = await context.Chapterbooks
                .Where(tp => tp.TeacherChapterbooks.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return chapterbooks;
        }

        #region helpers
        public static IQueryable<Chapterbook> ApplyFiltering(IQueryable<Chapterbook> query, ChapterbookQuery queryObj)
        {
            var term = queryObj.SearchTerm.ToLower().Trim();
            query = query.Where(c =>
            c.Title.ToLower().Contains(term) ||
            c.BookTitle.ToLower().Contains(term) ||
            c.ISBN.ToString().Contains(term) ||
            c.Editorial.ToLower().Contains(term));
            return query;
        }
        #endregion
    }
}