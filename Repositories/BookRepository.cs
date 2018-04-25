using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Extensions;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly RefcaDbContext _context;
        private readonly IMapper mapper;
        public BookRepository(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;
        }

        public async Task<Book> GetBook(int id)
        {
            return await _context.Books
                  .Include(tp => tp.TeacherBooks)
                      .ThenInclude(t => t.Teacher)
                  .SingleOrDefaultAsync(a => a.Id == id);

        }
        public void Add(Book book)
        {
            _context.Books.Add(book);
        }
        public void Remove(Book book)
        {
            _context.Remove(book);
        }

        public async Task<QueryResult<Book>> GetBooks(BookQuery queryObj)
        {
            var result = new QueryResult<Book>();
            var query = _context.Books
                  .Include(tp => tp.TeacherBooks)
                      .ThenInclude(t => t.Teacher)
                  .Where(p => p.IsApproved == true)
                  .OrderBy(d => d.AddedDate)
                  .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);


            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;

        }

        public async Task<QueryResult<Book>> GetAdminBooks(BookQuery queryObj)
        {
            var result = new QueryResult<Book>();
            var query = _context.Books
                  .Include(tp => tp.TeacherBooks)
                      .ThenInclude(t => t.Teacher)
                  .Where(p => p.IsApproved == queryObj.IsApproved)
                  .OrderBy(d => d.AddedDate)
                  .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;

        }

        public async Task<QueryResult<Book>> GetTeacherBooks(string userId, BookQuery queryObj)
        {
            var result = new QueryResult<Book>();
            var query = _context.Books
                .Where(tb => tb.TeacherBooks.Any(t => t.TeacherId == userId))
                .Include(tp => tp.TeacherBooks)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == queryObj.IsApproved)
                .OrderBy(d => d.AddedDate)
                .AsQueryable();

            if (!String.IsNullOrEmpty(queryObj.SearchTerm))
                query = ApplyFiltering(query, queryObj);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);
            await query.ForEachAsync(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            result.Items = await query.ToListAsync();

            return result;

        }

        public async Task<int> GetNumberOfBooksByTeacher(string userId)
        {
            var books = await _context.Books
                .Where(tp => tp.TeacherBooks.Any(t => t.TeacherId == userId))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return books;
        }
        
        #region helpers
        public static IQueryable<Book> ApplyFiltering(IQueryable<Book> query, BookQuery queryObj)
        {
            var term = queryObj.SearchTerm.ToLower().Trim();
            query = query.Where(a =>
            a.Title.ToLower().Contains(term) ||
            a.Editorial.ToLower().Contains(term) ||
            Convert.ToString(a.ISBN).Contains(term));
            return query;
        }
        #endregion
    }
}