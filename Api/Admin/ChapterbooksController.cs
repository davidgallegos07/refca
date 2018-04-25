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
using refca.Models;
using refca.Models.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using refca.Models.Identity;

namespace refca.Api.Admin

{
    [Route("api/admin/[controller]")]
    public class ChapterbooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IChapterbookRepository _chapterbookRepository;

        public ChapterbooksController(RefcaDbContext context, IMapper mapper, IChapterbookRepository _chapterbookRepository)
        {
            this._chapterbookRepository = _chapterbookRepository;
            this.mapper = mapper;
            _context = context;
        }

        // GET: api/chapterbooks?{query}
        [Authorize(Roles = Roles.Admin)]                
        [HttpGet]
        public async Task<QueryResultResource<ChapterbookResource>> GetChapterbooks(ChapterbookQueryResource filterResource)
        {
            var filter = mapper.Map<ChapterbookQueryResource, ChapterbookQuery>(filterResource);
            var queryResult = await _chapterbookRepository.GetAdminChapterbooks(filter);

            return mapper.Map<QueryResult<Chapterbook>, QueryResultResource<ChapterbookResource>>(queryResult);
        }
    }
}