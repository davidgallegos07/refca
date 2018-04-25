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

namespace refca.Api.Admin
{
    [Route("api/admin/[controller]")]
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
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<PresentationResource>> GetPresentations(PresentationQueryResource filterResource)
        {
            var filter = mapper.Map<PresentationQueryResource, PresentationQuery>(filterResource);
            var queryResult = await _presentationRepository.GetAdminPresentations(filter);

            return mapper.Map<QueryResult<Presentation>, QueryResultResource<PresentationResource>>(queryResult);
        }
    }
}