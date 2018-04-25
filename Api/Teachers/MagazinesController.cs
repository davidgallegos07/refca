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
    public class MagazinesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IMagazineRepository _magazineRepository;

        public MagazinesController(RefcaDbContext context, IMapper mapper,
        IMagazineRepository magazineRepository)
        {
            this.mapper = mapper;
            _context = context;
            _magazineRepository = magazineRepository;
        }

        // GET api/teachers/{userId}/magazines
        [HttpGet("{userId:guid}/magazines")]             
        public async Task<IActionResult> Magazines(string userId, TeacherMagazineQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherMagazineQueryResource, MagazineQuery>(filterResource);
            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == userId);
            if(teacher == null)
                return BadRequest();

            var queryResult = await _magazineRepository.GetTeacherMagazines(userId, filter);

            return Ok(mapper.Map<QueryResult<Magazine>, QueryResultResource<MagazineResource>>(queryResult));
        }

        // GET api/teachers/{userId}/magazines/count
        [HttpGet("{userId:guid}/magazines/count")]
        public async Task<IActionResult> GetNumberOfMagazinesByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _magazineRepository.GetNumberOfMagazinesByTeacher(userId));
        }
    }
}