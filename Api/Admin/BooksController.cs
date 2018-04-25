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

namespace refca.Api.Admin
{
    [Route("api/admin/[controller]")]
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
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<BookResource>> GetBooks(BookQueryResource filterResource)
        {

            var filter = mapper.Map<BookQueryResource, BookQuery>(filterResource);
            var queryResult = await _bookRepository.GetAdminBooks(filter);

            return mapper.Map<QueryResult<Book>, QueryResultResource<BookResource>>(queryResult);
        }

    }
}