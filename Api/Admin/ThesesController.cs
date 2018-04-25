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

namespace refca.Api.Admin
{
    [Route("api/admin/[controller]")]
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
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<ThesisResource>> GetTheses(ThesisQueryResource filterResource)
        {
            var filter = Mapper.Map<ThesisQueryResource, ThesisQuery>(filterResource);
            var queryResult = await _thesisRepository.GetAdminTheses(filter);

            return Mapper.Map<QueryResult<Thesis>, QueryResultResource<ThesisResource>>(queryResult);
        }
    }
}