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
    public class BooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IBookRepository _bookRepository;

        public BooksController(RefcaDbContext context, IMapper mapper,
        IBookRepository bookRepository)
        {
            this._bookRepository = bookRepository;
            this.mapper = mapper;
            _context = context;
        }
        
        // GET: api/teachers/{id}/books?{query}
        [HttpGet("{userId:guid}/books")]
        public async Task<IActionResult> GetBooks(string userId, TeacherBookQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherBookQueryResource, BookQuery>(filterResource);
            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == userId);
            if(teacher == null) return BadRequest();

            var queryResult = await _bookRepository.GetTeacherBooks(userId, filter);

            return Ok(mapper.Map<QueryResult<Book>, QueryResultResource<BookResource>>(queryResult));
        }

        // GET api/teachers/{id}/books/count
        [HttpGet("{userId:guid}/books/count")]
        public async Task<IActionResult> GetNumberOfBooksByTeacher(string userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
            if (teacher == null) return NotFound();

            return Ok(await _bookRepository.GetNumberOfBooksByTeacher(userId));
        }
    }
}