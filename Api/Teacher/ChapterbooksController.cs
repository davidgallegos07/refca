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
using Microsoft.AspNetCore.Identity;

namespace refca.Api.Teacher

{
    [Route("api/teacher/[controller]")]
    public class ChapterbooksController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IChapterbookRepository _chapterbookRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChapterbooksController(RefcaDbContext context, IMapper mapper,
        IChapterbookRepository _chapterbookRepository,  UserManager<ApplicationUser> userManager)
        {
            this._chapterbookRepository = _chapterbookRepository;
            this.mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        // GET: api/chapterbooks?{query}
        [Authorize(Roles = Roles.Teacher)]                
        [HttpGet]
        public async Task<QueryResultResource<ChapterbookResource>> GetChapterbooks(ChapterbookQueryResource filterResource)
        {
            var filter = mapper.Map<ChapterbookQueryResource, ChapterbookQuery>(filterResource);
            var userId =  _userManager.GetUserId(HttpContext.User);
            var queryResult = await _chapterbookRepository.GetTeacherChapterbooks(userId, filter);

            return mapper.Map<QueryResult<Chapterbook>, QueryResultResource<ChapterbookResource>>(queryResult);
        }
    }
}