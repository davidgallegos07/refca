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
using System.Net;
using System.Net.Http;
using refca.Resources.TeacherQueryResources;

namespace refca.Api.Teachers
{
    [Route("api/teachers")]
    public class ThesesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IThesisRepository _thesisRepository;

        public ThesesController(RefcaDbContext context, IMapper mapper,
        IThesisRepository thesisRepository)
        {
            this.mapper = mapper;
            this._thesisRepository = thesisRepository;
            _context = context;
        }

        // GET: api/teachers/{userId}/theses?{query}
        [HttpGet("{userId:guid}/theses")]
        public async Task<IActionResult> GetTheses(string userId, TeacherThesisQueryResource filterResource)
        {
            var filter = Mapper.Map<TeacherThesisQueryResource, ThesisQuery>(filterResource);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) 
                return BadRequest();

            var queryResult = await _thesisRepository.GetTeacherTheses(userId, filter);

            return Ok(Mapper.Map<QueryResult<Thesis>, QueryResultResource<ThesisResource>>(queryResult));
        }
        
        
        // GET api/teachers/{userId}/theses/count
        [HttpGet("{userId:guid}/theses/count")]
        public async Task<IActionResult> GetNumberOfThesesByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _thesisRepository.GetNumberOfThesesByTeacher(userId));
        }
    }
}