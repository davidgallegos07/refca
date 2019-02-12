using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Extensions;
using refca.Models;
using refca.Models.QueryFilters;
using Microsoft.AspNetCore.Http;
using refca.Resources;

namespace refca.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private RefcaDbContext context;
        private readonly IMapper mapper;
        private UserManager<ApplicationUser> userManager;
        public ArticleRepository(RefcaDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Article> GetArticle(int id)
        {
            return await context.Articles
                  .Include(tp => tp.TeacherArticles)
                      .ThenInclude(t => t.Teacher)
                  .SingleOrDefaultAsync(a => a.Id == id);

        }
        public void Add(Article article)
        {
            context.Articles.Add(article);
        }
        public void Remove(Article article)
        {
            context.Remove(article);
        }
        public async Task<QueryResult<Article>> GetArticles(ArticleQuery queryObj)
        {
            var result = new QueryResult<Article>();
            var query = context.Articles
                    .Include(tp => tp.TeacherArticles)
                        .ThenInclude(t => t.Teacher)
                    .Where(a => a.IsApproved == true)
                    .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Article>> GetAdminArticles(ArticleQuery queryObj)
        {
            var result = new QueryResult<Article>();
            var query = context.Articles
                    .Include(tp => tp.TeacherArticles)
                        .ThenInclude(t => t.Teacher)
                    .Where(a => a.IsApproved == queryObj.IsApproved)
                    .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            await query.ForEachAsync(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<QueryResult<Article>> GetTeacherArticles(string userId, ArticleQuery queryObj)
        {
            var result = new QueryResult<Article>();
            var query = context.Articles
                    .Where(ta => ta.TeacherArticles.Any(t => t.TeacherId == userId))
                    .Include(tp => tp.TeacherArticles)
                        .ThenInclude(t => t.Teacher)
                    .Where(a => a.IsApproved == queryObj.IsApproved)
                    .OrderBy(d => d.AddedDate)
                    .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            await query.ForEachAsync(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;
        }

        public async Task<int> GetNumberOfArticlesByTeacher(string userId)
        {
            var articles = await context.Articles
                .Where(tp => tp.TeacherArticles.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return articles;
        }

        #region helpers
        public static IQueryable<Article> ApplyFiltering(IQueryable<Article> query, ArticleQuery queryObj)
        {
            var term = queryObj.SearchTerm.ToLower().Trim();
            query = query.Where(a =>
            a.Title.ToLower().Contains(term) ||
            a.Magazine.ToLower().Contains(term) ||
            Convert.ToString(a.ISSN).Contains(term));

            return query;
        }
        #endregion
    }
}