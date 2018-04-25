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
    public class MagazinesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IMagazineRepository _magazineRepository;
        public MagazinesController(RefcaDbContext context, IMapper mapper, IMagazineRepository magazineRepository)
        {
            this.mapper = mapper;
            _context = context;
            _magazineRepository = magazineRepository;
        }

        // GET: api/magazines?{query}
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<MagazineResource>> GetMagazines(MagazineQueryResource filterResource)
        {
            var filter = mapper.Map<MagazineQueryResource, MagazineQuery>(filterResource);
            var queryResult = await _magazineRepository.GetAdminMagazines(filter);

            return mapper.Map<QueryResult<Magazine>, QueryResultResource<MagazineResource>>(queryResult);
        }
    }
}