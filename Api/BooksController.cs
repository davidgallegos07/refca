using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using refca.Dtos;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public IEnumerable<BookWithTeachersDto> Get()
        {
            var books = _context.Books
                .Include(tp => tp.TeacherBooks)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .OrderBy(d => d.AddedDate)
                .ToList();
                books.ForEach(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());
            return Mapper.Map<IEnumerable<BookWithTeachersDto>>(books);
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
        public async Task<IActionResult> Get(int id)
        {
            var book = await _context.Books
                .Include(tp => tp.TeacherBooks)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .SingleOrDefaultAsync(t => t.Id == id);
                book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList();

            if (book == null)
                return NotFound();

            return Ok(Mapper.Map<BookWithTeachersDto>(book));
        }
    }
}