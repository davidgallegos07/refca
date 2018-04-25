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
using Microsoft.AspNetCore.Identity;
using refca.Resources.TeacherQueryResources;

namespace refca.Api.Teachers
{
    [Route("api/teachers")]
    public class ResearchController : Controller
    {
        private RefcaDbContext _context;
        private IResearchRepository _researchRepository;
        private readonly IMapper mapper;
        
        public ResearchController(RefcaDbContext context, IMapper mapper,
        IResearchRepository researchRepository)
        {
            this.mapper = mapper;
            _context = context;
            _researchRepository = researchRepository;
        }

        // GET api/teachers/{userId}/research
        [HttpGet("{userId:guid}/research")]
        public async Task<IActionResult> GetResearch(string userId, TeacherResearchQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherResearchQueryResource, ResearchQuery>(filterResource);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null)
                return NotFound();

            var queryResult = await _researchRepository.GetTeacherResearch(userId, filter);

            return Ok(mapper.Map<QueryResult<Research>, QueryResultResource<ResearchResource>>(queryResult));
        }

        // GET api/teachers/{userId}/research/count
        [HttpGet("{userId:guid}/research/count")]
        public async Task<IActionResult> GetNumberOfResearchByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _researchRepository.GetNumberOfResearchByTeacher(userId));
        }
    }
}