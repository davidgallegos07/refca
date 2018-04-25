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
using refca.Resources.TeacherResources;
namespace refca.Api.Teacher
{
    [Route("api/teacher/[controller]")]
    [Authorize]
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

        // GET: api/teacher/articles?{query}
        [Authorize(Roles = Roles.Teacher)]                
        [HttpGet]
        public async Task<QueryResultResource<ArticleResource>> GetArticles(ArticleQueryResource filterResource)
        {
            var filter = mapper.Map<ArticleQueryResource, ArticleQuery>(filterResource);
            var userId =  _userManager.GetUserId(HttpContext.User);

            var queryResult = await _articleRepository.GetTeacherArticles(userId, filter);

            return mapper.Map<QueryResult<Article>, QueryResultResource<ArticleResource>>(queryResult);
        }

        // GET api/teacher/articles/{id}/role
        [Authorize(Roles = Roles.Teacher)]  
        [HttpGet("{id}/Role")]
        public IActionResult GetArticleRoleTeacher(int id)
        {
            var userId = _userManager.GetUserId(User);
            return Ok(_context.TeacherArticles.Where(t => t.ArticleId == id && t.TeacherId == userId)
                    .Select(r => r.Role));
        }
    }
}