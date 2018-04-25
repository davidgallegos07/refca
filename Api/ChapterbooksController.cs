using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using refca.Resources;
using refca.Repositories;
using refca.Resources.QueryResources;
using refca.Models;
using refca.Models.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using refca.Models.Identity;
using refca.Resources.TeacherQueryResources;

namespace refca.Api

{
    [Route("api/[controller]")]
    public class ChapterbooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IChapterbookRepository _chapterbookRepository;

        public ChapterbooksController(RefcaDbContext context, IMapper mapper, IChapterbookRepository _chapterbookRepository)
        {
            this._chapterbookRepository = _chapterbookRepository;
            this.mapper = mapper;
            _context = context;
        }

        // GET: api/chapterbooks?{query}
        [HttpGet]
        public async Task<QueryResultResource<ChapterbookResource>> GetChapterbooks(TeacherChapterbookQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherChapterbookQueryResource, ChapterbookQuery>(filterResource);
            var queryResult = await _chapterbookRepository.GetChapterbooks(filter);

            return mapper.Map<QueryResult<Chapterbook>, QueryResultResource<ChapterbookResource>>(queryResult);
        }

        // GET: api/chapterbooks/count

        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfchapterbooks()
        {
            var chapterbook = await _context.Chapterbooks.Where(p => p.IsApproved == true).CountAsync();
            return Ok(chapterbook);
        }

        // GET api/chapterbooks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var chapterbook = await _context.Chapterbooks
                 .Include(tp => tp.TeacherChapterbooks)
                   .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (chapterbook == null)
                return NotFound();

            chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<ChapterbookResource>(chapterbook));
        }
    }
}