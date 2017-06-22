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
    public class ChapterbooksController : Controller
    {
        private ApplicationDbContext _context;

        public ChapterbooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/chapterbooks
        [HttpGet]
        public IEnumerable<ChapterbookWithTeachersDto> Get()
        {
            var chapterbooks = _context.Chapterbooks
                 .Include(tp => tp.TeacherChapterbooks)
                     .ThenInclude(t => t.Teacher)
                 .Where(p => p.IsApproved == true)
                 .OrderBy(d => d.AddedDate)
                 .ToList();
                chapterbooks.ForEach(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

           return Mapper.Map<IEnumerable<ChapterbookWithTeachersDto>>(chapterbooks);
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
                chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList();

            if (chapterbook == null)
                return NotFound();

            return Ok(Mapper.Map<ChapterbookWithTeachersDto>(chapterbook));
        }
    }
}