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
using refca.Models.QueryFilters;
using refca.Resources.QueryResources;
using refca.Models;
using refca.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using refca.Resources.TeacherQueryResources;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class ThesesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IThesisRepository _thesisRepository;

        public ThesesController(RefcaDbContext context, IMapper mapper, IThesisRepository thesisRepository)
        {
            this.mapper = mapper;
            this._thesisRepository = thesisRepository;
            _context = context;
        }

        // GET: api/theses?{query}
        [HttpGet]
        public async Task<QueryResultResource<ThesisResource>> GetTheses(TeacherThesisQueryResource filterResource)
        {
            var filter = Mapper.Map<TeacherThesisQueryResource, ThesisQuery>(filterResource);
            var queryResult = await _thesisRepository.GetTheses(filter);

            return Mapper.Map<QueryResult<Thesis>, QueryResultResource<ThesisResource>>(queryResult);
        }

        // GET: api/theses/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfByTheses()
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
                .Include(e => e.EducationProgram)
                .Include(r => r.ResearchLine)
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (thesis == null)
                return NotFound();

            thesis.TeacherTheses = thesis.TeacherTheses.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<ThesisResource>(thesis));
        }

        // POST api/GetResearchLines/{id}
        [HttpPost("GetResearchLines")]
        public async Task<IActionResult> GetResearchLines(int id)
        {
            var educationProgram = await _context.EducationPrograms.SingleOrDefaultAsync(ep => ep.Id == id);
            if (educationProgram == null)
                return NotFound();

            var researchLine = _context.EducationProgramResearchLine
                                .Where(r => r.EducationProgramId == educationProgram.Id)
                                .Select(r => r.ResearchLine);

            return Ok(mapper.Map<IEnumerable<ResearchLineResource>>(researchLine));
        }
    }
}