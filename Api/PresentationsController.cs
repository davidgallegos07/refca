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
    public class PresentationsController : Controller
    {
        private ApplicationDbContext _context;

        public PresentationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/presentations
        [HttpGet]
        public IEnumerable<PresentationWithTeachersDto> Get()
        {
            var presentations =  _context.Presentations
               .Include(tp => tp.TeacherPresentations)
                   .ThenInclude(t => t.Teacher)
               .Where(p => p.IsApproved == true)
               .OrderBy(d => d.AddedDate)
               .ToList();
                presentations.ForEach(presentation => presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList());

            return Mapper.Map<IEnumerable<PresentationWithTeachersDto>>(presentations);
        }

        // GET: api/presentations/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfPresentations()
        {
            var presentation = await _context.Thesis.Where(a => a.IsApproved == true).CountAsync();
            return Ok(presentation);
        }

        // GET api/presentations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var presentation = await _context.Presentations
                 .Include(tp => tp.TeacherPresentations)
                    .ThenInclude(t => t.Teacher)
                 .Where(p => p.IsApproved == true)
                 .SingleOrDefaultAsync(t => t.Id == id);
                 presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList();

            if (presentation == null)
                return NotFound();

            return Ok(Mapper.Map<PresentationWithTeachersDto>(presentation));
        }
    }
}