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
using Microsoft.AspNetCore.Identity;

namespace refca.Repositories
{
    public class ResearchRepository : IResearchRepository
    {
        private  RefcaDbContext context;
        private readonly IMapper mapper;
        private UserManager<ApplicationUser> UserManager;

        public ResearchRepository(RefcaDbContext context, IMapper mapper, UserManager<ApplicationUser> UserManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.UserManager = UserManager;
        }

         public async Task<Research> GetResearch(int id)
            {
                return await context.Research
                    .Include(tp => tp.TeacherResearch)
                        .ThenInclude(t => t.Teacher)
                    .SingleOrDefaultAsync(a => a.Id ==id);
            }
          public void Add(Research research)
        {
            context.Research.Add(research);
        }

        public void Remove(Research research)
        {
            context.Remove(research);
        }

       
        public async Task<QueryResult<Research>> GetResearch(ResearchQuery queryObj)
        {
            var result = new QueryResult<Research>();

            var query = context.Research
               .Include(tp => tp.TeacherResearch)
                   .ThenInclude(t => t.Teacher)
               .Include(r => r.ResearchLine)
               .Include(k => k.KnowledgeArea)
               .Include(a => a.AcademicBody)
               .Where(p => p.IsApproved == true)
               .OrderBy(d => d.AddedDate)
               .AsQueryable();

            query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            await query.ForEachAsync(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;

        }

        public async Task<QueryResult<Research>> GetAdminResearch(ResearchQuery queryObj)
        {
            var result = new QueryResult<Research>();

            var query = context.Research
                .Include(tp => tp.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();

            query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            await query.ForEachAsync(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;

        }

        public async Task<QueryResult<Research>> GetTeacherResearch(string userId, ResearchQuery queryObj)
        {
            var result = new QueryResult<Research>();

            var query = context.Research
                .Where(tr => tr.TeacherResearch.Any(t => t.TeacherId == userId))
                .Include(tp => tp.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();

            query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            await query.ForEachAsync(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;

        }
        public async Task<int> GetNumberOfResearchByTeacher(string userId)
        {
            var research = await context.Research
                .Where(tp => tp.TeacherResearch.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return research;
        }


        #region helpers
        public static IQueryable<Research> ApplyFiltering(IQueryable<Research> query, ResearchQuery queryObj)
        {
            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
            {
                var term = queryObj.SearchTerm.ToLower();
                query = query.Where(r =>
                r.Title.ToLower().Contains(term) ||
                r.Code.ToLower().Contains(term) ||
                r.Sector.ToLower().Contains(term));
            }
            if (queryObj.AcademicBodyId.HasValue)
                query = query.Where(a =>
                a.AcademicBodyId == queryObj.AcademicBodyId.Value);

            if (queryObj.ResearchLineId.HasValue)
                query = query.Where(r =>
                r.ResearchLineId == queryObj.ResearchLineId.Value);

            if (queryObj.KnowledgeAreaId.HasValue)
                query = query.Where(k =>
                k.KnowledgeAreaId == queryObj.KnowledgeAreaId.Value);

            return query;
        }

      
        #endregion
    }
}