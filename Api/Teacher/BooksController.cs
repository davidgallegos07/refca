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
    public class BooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IBookRepository _bookRepository;
        private UserManager<ApplicationUser> _userManager;

        public BooksController(RefcaDbContext context, IMapper mapper,
        IBookRepository bookRepository,  UserManager<ApplicationUser> userManager)
        {
            this._bookRepository = bookRepository;
            this.mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        // GET: api/books?{query}
        [Authorize(Roles = Roles.Teacher)]                
        [HttpGet]
        public async Task<QueryResultResource<BookResource>> GetBooks(BookQueryResource filterResource)

        {

            var filter = mapper.Map<BookQueryResource, BookQuery>(filterResource);
            var userId =  _userManager.GetUserId(HttpContext.User);

            var queryResult = await _bookRepository.GetTeacherBooks(userId, filter);

            return mapper.Map<QueryResult<Book>, QueryResultResource<BookResource>>(queryResult);
        }

        // GET api/teacher/books/{id}/role
        [Authorize(Roles = Roles.Teacher)]  
        [HttpGet("{id}/Role")]
        public IActionResult GetBookRoleTeacher(int id)
        {
            var userId = _userManager.GetUserId(User);
            return Ok(_context.TeacherBooks.Where(t => t.BookId== id && t.TeacherId == userId)
                    .Select(r => r.Role));
        }
    }
}