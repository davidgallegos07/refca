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
using Microsoft.AspNetCore.Identity;
using refca.Resources.TeacherQueryResources;

namespace refca.Api.Teachers

{
    [Route("api/teachers")]
    public class ChapterbooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IChapterbookRepository _chapterbookRepository;

        public ChapterbooksController(RefcaDbContext context, IMapper mapper,
        IChapterbookRepository _chapterbookRepository,  UserManager<ApplicationUser> userManager)
        {
            this._chapterbookRepository = _chapterbookRepository;
            this.mapper = mapper;
            _context = context;
        }

        // GET: api/teachers/{userId}/chapterbooks?{query}
        [HttpGet("{userId:guid}/chapterbooks")]
        public async Task<IActionResult> GetChapterbooks(string userId, TeacherChapterbookQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherChapterbookQueryResource, ChapterbookQuery>(filterResource);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null)
                return NotFound();

            var queryResult = await _chapterbookRepository.GetTeacherChapterbooks(userId, filter);

            return Ok(mapper.Map<QueryResult<Chapterbook>, QueryResultResource<ChapterbookResource>>(queryResult));
        }
        
        // GET api/teachers/{userId}/chapterbooks/count
        [HttpGet("{userId:guid}/chapterbooks/count")]
        public async Task<IActionResult> GetNumberOfChapterbooksByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _chapterbookRepository.GetNumberOfChapterbooksByTeacher(userId));
        }
    }
}