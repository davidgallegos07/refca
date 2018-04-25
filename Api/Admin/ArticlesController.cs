using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using refca.Data;
using AutoMapper;
using refca.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using refca.Repositories;
using refca.Models;
using Microsoft.AspNetCore.Authorization;
using refca.Models.Identity;
using refca.Resources.QueryResources;
using refca.Models.QueryFilters;

namespace refca.Api.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize]
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

        // GET: api/admin/articles?{query}
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<ArticleResource>> GetArticles(ArticleQueryResource filterResource)
        {
            var filter = mapper.Map<ArticleQueryResource, ArticleQuery>(filterResource);
            var queryResult = await _articleRepository.GetAdminArticles(filter);

            return mapper.Map<QueryResult<Article>, QueryResultResource<ArticleResource>>(queryResult);
            //return mapper.Map<QueryResult<ArticleResource>, QueryResultResource<ArticleResource>>(queryResult);

        }

    }
}