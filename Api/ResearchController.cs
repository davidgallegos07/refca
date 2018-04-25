using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using AutoMapper;
using refca.Resources;
using Microsoft.EntityFrameworkCore;
using refca.Repositories;
using refca.Models;
using refca.Resources.QueryResources;
using refca.Models.QueryFilters;
using refca.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using refca.Resources.TeacherQueryResources;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class ResearchController : Controller
    {
        private RefcaDbContext _context;
        private IResearchRepository _researchRepository;
        private readonly IMapper mapper;
        public ResearchController(RefcaDbContext context,
        IResearchRepository researchRepository, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
            _researchRepository = researchRepository;
        }

        // GET: api/research?{query}
        [HttpGet]
        public async Task<QueryResultResource<ResearchResource>> GetResearch(TeacherResearchQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherResearchQueryResource, ResearchQuery>(filterResource);
            var queryResult = await _researchRepository.GetResearch(filter);

            return mapper.Map<QueryResult<Research>, QueryResultResource<ResearchResource>>(queryResult);
        }

        // GET: api/research/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfResearch()
        {
            var research = await _context.Research.Where(p => p.IsApproved == true).CountAsync();
            return Ok(research);
        }

        // GET: api/research/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var research = await _context.Research
                .Include(tp => tp.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (research == null)
                return NotFound();

            research.TeacherResearch = research.TeacherResearch.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<ResearchResource>(research));
        }

        // POST: api/research/getresearchlines/{id}
        [HttpPost("GetResearchLines")]
        public async Task<IActionResult> GetResearchLines(int id)
        {
            var academicBody = await _context.AcademicBodies.SingleOrDefaultAsync(ab => ab.Id == id);
            if (academicBody == null)
                return NotFound();

            var researchLine = await _context.ResearchLines.Where(ab => ab.AcademicBodyId == academicBody.Id)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<ResearchLineResource>>(researchLine));
        }

    }
}