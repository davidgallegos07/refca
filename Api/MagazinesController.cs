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
    public class MagazinesController : Controller
    {
        private RefcaDbContext _context;
        private readonly IMapper mapper;
        private readonly IMagazineRepository _magazineRepository;
        public MagazinesController(RefcaDbContext context, IMapper mapper, IMagazineRepository magazineRepository)
        {
            this.mapper = mapper;
            _context = context;
            _magazineRepository = magazineRepository;
        }

        // GET: api/magazines?{query}
        [HttpGet]
        public async Task<QueryResultResource<MagazineResource>> GetMagazines(TeacherMagazineQueryResource filterResource)
        {
            var filter = mapper.Map<TeacherMagazineQueryResource, MagazineQuery>(filterResource);
            var queryResult = await _magazineRepository.GetMagazines(filter);

            return mapper.Map<QueryResult<Magazine>, QueryResultResource<MagazineResource>>(queryResult);
        }

        // GET: api/magazines/count
        [HttpGet("Count")]
        public async Task<IActionResult> GetNumberOfMagazines()
        {
            var magazine = await _context.Magazines.Where(p => p.IsApproved == true).CountAsync();
            return Ok(magazine);
        }

        // GET api/magazines/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var magazine = await _context.Magazines
                 .Include(tp => tp.TeacherMagazines)
                    .ThenInclude(t => t.Teacher)
                 .Where(p => p.IsApproved == true)
                 .SingleOrDefaultAsync(t => t.Id == id);

            if (magazine == null)
                return NotFound();

            magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList();

            return Ok(mapper.Map<MagazineResource>(magazine));
        }
    }
}