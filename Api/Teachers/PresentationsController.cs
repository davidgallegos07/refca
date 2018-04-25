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
using Microsoft.AspNetCore.Identity;
using refca.Resources.TeacherQueryResources;

namespace refca.Api.Teachers
{
    [Route("api/teachers")]
    public class PresentationsController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IPresentationRepository _presentationRepository;
        
        public PresentationsController(RefcaDbContext context, IMapper mapper, 
        IPresentationRepository presentationRepository)
        {
            this.mapper = mapper;
            _context = context;
            _presentationRepository = presentationRepository;
        }

        // GET: api/teachers/{userId}/presentations?{query}
        [HttpGet("{userId:guid}/presentations")]
        public async Task<IActionResult> GetPresentations(string userId, TeacherPresentationQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherPresentationQueryResource, PresentationQuery>(filterResource);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null)
                return NotFound();

            var queryResult = await _presentationRepository.GetTeacherPresentations(userId, filter);

            return Ok(mapper.Map<QueryResult<Presentation>, QueryResultResource<PresentationResource>>(queryResult));
        }

        // GET api/teachers/{userId}/presentations/count
        [HttpGet("{userId:guid}/presentations/count")]
        public async Task<IActionResult> GetNumberOfPresentationsByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _presentationRepository.GetNumberOfPresentationsByTeacher(userId));
        }
    }
}