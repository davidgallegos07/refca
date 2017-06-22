using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using AutoMapper;
using refca.Dtos;
using Microsoft.EntityFrameworkCore;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class ResearchController : Controller
    {
        private ApplicationDbContext _context;
        public ResearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/research
        [HttpGet]
        public IEnumerable<ResearchWithTeachersDto> Get()
        {
            var research = _context.Research
                .Include(tp => tp.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Where(p => p.IsApproved == true)
                .OrderBy(d => d.AddedDate)
                .ToList();
                research.ForEach(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            return Mapper.Map<IEnumerable<ResearchWithTeachersDto>>(research);
        }

        // GET: api/research/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfResearch()
        {
            var research = await _context.Research.Where(p => p.IsApproved == true).CountAsync();
            return Ok(research);
        }

        // GET api/research/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var research = await _context.Research
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (research == null)
                return NotFound();
                 
            research.TeacherResearch = research.TeacherResearch.OrderBy(o => o.Order).ToList();

            return Ok(Mapper.Map<ResearchWithTeachersDto>(research));
        }

        // POST api/research/getresearchlines/{id}
        [HttpPost("GetResearchLines")]
        public async Task<IActionResult> GetResearchLines(int id)
        {
            var academicBody = await _context.AcademicBodies.SingleOrDefaultAsync(ab => ab.Id == id);
            if (academicBody == null)
                return NotFound();

            var researchLine = await _context.ResearchLines.Where(ab => ab.AcademicBodyId == academicBody.Id)
                .ToListAsync();
            return Ok(Mapper.Map<IEnumerable<ResearchLineDto>>(researchLine));
        }

    }
}