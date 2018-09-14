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
    public class PresentationsController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IPresentationRepository _presentationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PresentationsController(RefcaDbContext context, IMapper mapper,
        IPresentationRepository presentationRepository, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            _context = context;
            _presentationRepository = presentationRepository;
            _userManager = userManager;
        }

        // GET: api/presentations?{query}
        [Authorize(Roles = Roles.Teacher)]
        [HttpGet]
        public async Task<QueryResultResource<PresentationResource>> GetPresentations(PresentationQueryResource filterResource)
        {
            var filter = mapper.Map<PresentationQueryResource, PresentationQuery>(filterResource);
            var userId = _userManager.GetUserId(HttpContext.User);
            var queryResult = await _presentationRepository.GetTeacherPresentations(userId, filter);

            return mapper.Map<QueryResult<Presentation>, QueryResultResource<PresentationResource>>(queryResult);
        }

        // GET api/teacher/presentations/{id}/role
        [Authorize(Roles = Roles.Teacher)]
        [HttpGet("{id}/Role")]
        public IActionResult GetPresentationRoleTeacher(int id)
        {
            var userId = _userManager.GetUserId(User);
            return Ok(_context.TeacherPresentations.Where(t => t.PresentationId == id && t.TeacherId == userId)
                    .Select(r => r.Role));
        }
    }
}