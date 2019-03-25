using Microsoft.AspNetCore.Mvc;
using refca.Data;
using AutoMapper;
using refca.Resources;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class AcademicBodies : Controller
    {
        private readonly RefcaDbContext _context;
        private readonly IMapper mapper;
        public AcademicBodies(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;
        }

        // GET: api/academicbodies
        [HttpGet]
        public async Task<IEnumerable<AcademicBodyResource>> Get()
        {
            var academicBodies = await _context.AcademicBodies.ToListAsync();
            return mapper.Map<IEnumerable<AcademicBodyResource>>(academicBodies);
        }

        
    }
}