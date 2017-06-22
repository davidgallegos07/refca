using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using refca.Dtos;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/articles
        [HttpGet]
        public IEnumerable<ArticleWithTeachersDto> Get()
        {
            var articles =  _context.Articles
               .Include(tp => tp.TeacherArticles)
                   .ThenInclude(t => t.Teacher)
               .Where(p => p.IsApproved == true)
               .OrderBy(d => d.AddedDate)
               .ToList();
                articles.ForEach(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());

            return Mapper.Map<IEnumerable<ArticleWithTeachersDto>>(articles);
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
                 article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList();

            if (article == null)
                return NotFound();

            return Ok(Mapper.Map<ArticleWithTeachersDto>(article));
        }
    }
}