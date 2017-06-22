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
    public class ThesesController : Controller
    {
        private ApplicationDbContext _context;

        public ThesesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/theses
        [HttpGet]
        public IEnumerable<ThesisWithTeachersDto> Get()
        {
            var theses = _context.Thesis
                 .Include(tp => tp.TeacherTheses)
                   .ThenInclude(t => t.Teacher)
               .Include(e => e.EducationProgram)
               .Include(r => r.ResearchLine)
               .Where(p => p.IsApproved == true)
               .OrderBy(d => d.AddedDate)
               .ToList();
                theses.ForEach(thesis => thesis.TeacherTheses = thesis.TeacherTheses.OrderBy(o => o.Order).ToList());

            return Mapper.Map<IEnumerable<ThesisWithTeachersDto>>(theses);
        }

        // GET: api/theses/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfThesis()
        {
            var theses = await _context.Thesis.Where(p => p.IsApproved == true).CountAsync();
            return Ok(theses);
        }

        // GET api/theses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var thesis = await _context.Thesis
                .Include(tp => tp.TeacherTheses)
                   .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (thesis == null)
                return NotFound();

            thesis.TeacherTheses = thesis.TeacherTheses.OrderBy(o => o.Order).ToList();

            return Ok(Mapper.Map<ThesisWithTeachersDto>(thesis));
        }

        // POST api/GetResearchLines/{id}
        [HttpPost("GetResearchLines")]
        public async Task<IActionResult> GetResearchLines(int id)
        {
            var educationProgram = await _context.EducationPrograms.SingleOrDefaultAsync(ep => ep.Id == id);
            if (educationProgram == null)
                return NotFound();

            var researchLine =  _context.EducationProgramResearchLine
                                .Where(r => r.EducationProgramId == educationProgram.Id)
                                .Select(r => r.ResearchLine);

            return Ok(Mapper.Map<IEnumerable<ResearchLineDto>>(researchLine));
        }
    }
}