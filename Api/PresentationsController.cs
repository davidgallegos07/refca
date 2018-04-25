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
using refca.Models.QueryFilters;
using refca.Models;
using Microsoft.AspNetCore.Authorization;
using refca.Models.Identity;
using refca.Resources.TeacherQueryResources;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class PresentationsController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IPresentationRepository _presentationRepository;
        public PresentationsController(RefcaDbContext context, IMapper mapper, IPresentationRepository presentationRepository)
        {
            this.mapper = mapper;
            _context = context;
            _presentationRepository = presentationRepository;
        }

        // GET: api/presentations?{query}
        [HttpGet]
        public async Task<QueryResultResource<PresentationResource>> GetPresentations(TeacherPresentationQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherPresentationQueryResource, PresentationQuery>(filterResource);
            var queryResult = await _presentationRepository.GetPresentations(filter);

            return mapper.Map<QueryResult<Presentation>, QueryResultResource<PresentationResource>>(queryResult);
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

            if (presentation == null)
                return NotFound();

            presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<PresentationResource>(presentation));
        }
    }
}