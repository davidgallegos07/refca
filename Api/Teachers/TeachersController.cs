using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using refca.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using refca.Resources;
using System.Threading.Tasks;
using refca.Repositories;
using refca.Resources.TeacherQueryResources;
using refca.Extensions;
using refca.Resources.QueryResources;
using refca.Models.QueryFilters;

namespace refca.Api.Teachers
{
    [Route("api/[controller]")]
    public class TeachersController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly ITeacherRepository _teacherRepository;
        public TeachersController(RefcaDbContext context, IMapper mapper, ITeacherRepository teacherRepository)
        {
            this.mapper = mapper;
            _context = context;
            _teacherRepository = teacherRepository;
        }

        //GET: api/teachers
        [HttpGet]
        public async Task<QueryResultResource<TeacherResource>> GetTeachers(TeacherQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherQueryResource, TeacherQuery>(filterResource);
            var teachers = await _teacherRepository.GetTeachers(filter);
            return mapper.Map<QueryResultResource<TeacherResource>>(teachers);
        }

        // GET: api/teachers/search/{searchTerm}
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> GetTeachers(string searchTerm)
        {
            var search = await _context.Teachers
                .Where(c => c.Name.Contains(searchTerm)).ToListAsync();

            return Ok(mapper.Map<IEnumerable<TeacherResource>>(search));
        }

        // GET api/teachers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTeacher(string id)
        {
            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            return Ok(mapper.Map<TeacherResource>(teacher));
        }
    }
}