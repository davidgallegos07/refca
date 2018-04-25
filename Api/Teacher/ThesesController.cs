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
using Microsoft.AspNetCore.Identity;

namespace refca.Api.Teacher
{
    [Route("api/teacher/[controller]")]
    public class ThesesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IThesisRepository _thesisRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThesesController(RefcaDbContext context, IMapper mapper,
        IThesisRepository thesisRepository, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this._thesisRepository = thesisRepository;
            _context = context;
            _userManager = userManager;
        }

        // GET: api/theses?{query}
        [Authorize(Roles = Roles.Teacher)]                
        [HttpGet]
        public async Task<QueryResultResource<ThesisResource>> GetTheses(ThesisQueryResource filterResource)
        {
            var filter = Mapper.Map<ThesisQueryResource, ThesisQuery>(filterResource);
            var userId =  _userManager.GetUserId(HttpContext.User);
            var queryResult = await _thesisRepository.GetTeacherTheses(userId, filter);

            return Mapper.Map<QueryResult<Thesis>, QueryResultResource<ThesisResource>>(queryResult);
        }
    }
}