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

namespace refca.Api.Teacher
{
    [Route("api/teacher/[controller]")]
    public class ResearchController : Controller
    {
        private RefcaDbContext _context;
        private IResearchRepository _researchRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ResearchController(RefcaDbContext context, IMapper mapper,
        IResearchRepository researchRepository, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            _context = context;
            _researchRepository = researchRepository;
            _userManager = userManager;
        }

        // GET: api/research?{query}
        [Authorize(Roles = Roles.Teacher)]                
        [HttpGet]
        public async Task<QueryResultResource<ResearchResource>> GetResearch(ResearchQueryResource filterResource)
        {
            var filter = mapper.Map<ResearchQueryResource, ResearchQuery>(filterResource);
            var userId =  _userManager.GetUserId(HttpContext.User);
            var queryResult = await _researchRepository.GetTeacherResearch(userId, filter);

            return mapper.Map<QueryResult<Research>, QueryResultResource<ResearchResource>>(queryResult);
        }
        
    }
}