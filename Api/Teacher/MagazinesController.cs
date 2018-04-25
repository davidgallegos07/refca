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

namespace refca.Api.Teacher
{
    [Route("api/teacher/[controller]")]
    public class MagazinesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IMagazineRepository _magazineRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public MagazinesController(RefcaDbContext context, IMapper mapper,
        IMagazineRepository magazineRepository, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            _context = context;
            _magazineRepository = magazineRepository;
            _userManager = userManager;
        }

        // GET: api/magazines?{query}
        [Authorize(Roles = Roles.Teacher)]                
        [HttpGet]
        public async Task<QueryResultResource<MagazineResource>> GetMagazines(MagazineQueryResource filterResource)
        {
            var filter = mapper.Map<MagazineQueryResource, MagazineQuery>(filterResource);
            var userId =  _userManager.GetUserId(HttpContext.User);
            var queryResult = await _magazineRepository.GetTeacherMagazines(userId, filter);

            return mapper.Map<QueryResult<Magazine>, QueryResultResource<MagazineResource>>(queryResult);
        }
    }
}