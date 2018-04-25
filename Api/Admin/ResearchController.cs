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

namespace refca.Api.Admin
{
    [Route("api/admin/[controller]")]
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
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<ResearchResource>> GetResearch(ResearchQueryResource filterResource)
        {
            var filter = mapper.Map<ResearchQueryResource, ResearchQuery>(filterResource);
            var queryResult = await _researchRepository.GetAdminResearch(filter);

            return mapper.Map<QueryResult<Research>, QueryResultResource<ResearchResource>>(queryResult);
        }
    }
}