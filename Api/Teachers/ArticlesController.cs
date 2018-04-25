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
using Microsoft.AspNetCore.Identity;
using refca.Resources.TeacherQueryResources;

namespace refca.Api.Teachers
{
    [Route("api/teachers")]
    public class ArticlesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IArticleRepository _articleRepository;
        private UserManager<ApplicationUser> _userManager;
        
        public ArticlesController(RefcaDbContext context, IMapper mapper, 
        IArticleRepository articleRepository, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            _context = context;
            _articleRepository = articleRepository;
            _userManager = userManager;
        }

        // GET: api/teachers/{userId}/articles?{query}
        [HttpGet("{userId:guid}/articles")]
        public async Task<IActionResult> GetArticles(string userId, TeacherArticleQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherArticleQueryResource, ArticleQuery>(filterResource);
            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == userId);
            if(teacher == null)
                return BadRequest();

            var queryResult = await _articleRepository.GetTeacherArticles(userId, filter);

            return Ok(mapper.Map<QueryResult<Article>, QueryResultResource<ArticleResource>>(queryResult));
            //return Ok(mapper.Map<QueryResult<ArticleResource>, QueryResultResource<ArticleResource>>(queryResult));

        }

        // GET api/teachers/{id}/articles/count
        [HttpGet("{userId:guid}/articles/count")]
        public async Task<IActionResult> GetNumberOfArticlesByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _articleRepository.GetNumberOfArticlesByTeacher(userId));
        }
    }
}