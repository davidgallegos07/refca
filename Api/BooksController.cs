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
using refca.Resources.TeacherQueryResources;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IBookRepository _bookRepository;

        public BooksController(RefcaDbContext context, IMapper mapper, IBookRepository bookRepository)
        {
            this._bookRepository = bookRepository;
            this.mapper = mapper;
            _context = context;
        }

        // GET: api/books?{query}
        [HttpGet]
        public async Task<QueryResultResource<BookResource>> GetBooks(TeacherBookQueryResource filterResource)
        {

            var filter = mapper.Map<TeacherBookQueryResource, BookQuery>(filterResource);
            var queryResult = await _bookRepository.GetBooks(filter);

            return mapper.Map<QueryResult<Book>, QueryResultResource<BookResource>>(queryResult);
        }

        // GET: api/books/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfBooks()
        {
            var book = await _context.Books.Where(p => p.IsApproved == true).CountAsync();
            return Ok(book);
        }

        // GET api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books
                .Include(tp => tp.TeacherBooks)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (book == null)
                return NotFound();

            book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<BookResource>(book));
        }
    }
}