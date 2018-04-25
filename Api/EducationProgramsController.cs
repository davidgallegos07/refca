using System.Linq;
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
    public class EducationPrograms : Controller
    {
        private readonly RefcaDbContext _context;
        private readonly IMapper mapper;
        public EducationPrograms(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;
        }

        // GET: api/educationPrograms
        [HttpGet]
        public async Task<IEnumerable<EducationProgramResource>> Get()
        {
            var educationPrograms = await _context.EducationPrograms.ToListAsync();
            return mapper.Map<IEnumerable<EducationProgramResource>>(educationPrograms);
        }
    }
}