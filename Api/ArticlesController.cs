using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using refca.Data;
using AutoMapper;
using refca.Resources;
using refca.Resources.TeacherResources;
using refca.Resources.TeacherQueryResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using refca.Repositories;
using refca.Models;
using Microsoft.AspNetCore.Authorization;
using refca.Models.Identity;
using refca.Models.QueryFilters;
using refca.Core;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IArticleRepository _articleRepository;
        public ArticlesController(RefcaDbContext context, IMapper mapper, IArticleRepository articleRepository)
        {
            this.mapper = mapper;
            _context = context;
            _articleRepository = articleRepository;
        }
        
        // GET: api/articles?{query}
        [HttpGet]
        public async Task<QueryResultResource<ArticleResource>> GetArticles(TeacherArticleQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherArticleQueryResource, ArticleQuery>(filterResource);
            var queryResult = await _articleRepository.GetArticles(filter);

            return mapper.Map<QueryResult<Article>, QueryResultResource<ArticleResource>>(queryResult);
            //return mapper.Map<QueryResult<ArticleResource>, QueryResultResource<ArticleResource>>(queryResult);            
        }

        // GET: api/articles/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfArticles()
        {
            var articles = await _context.Thesis.Where(a => a.IsApproved == true).CountAsync();
            return Ok(articles);
        }

        // GET api/articles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var article = await _context.Articles
                 .Include(tp => tp.TeacherArticles)
                    .ThenInclude(t => t.Teacher)
                 .Where(p => p.IsApproved == true)
                 .SingleOrDefaultAsync(t => t.Id == id);

            if (article == null)
                return NotFound();

            article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<ArticleResource>(article));
        }
    }
}