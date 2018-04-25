using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using refca.Resources;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class ResearchLines : Controller
    {
        private readonly RefcaDbContext _context;
        private readonly IMapper mapper;
        public ResearchLines(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;
        }

        // GET: api/ResearchLines
        [HttpGet]
        public IEnumerable<ResearchLineResource> Get()
        {
            var researchLines = _context.ResearchLines.ToList();
            return mapper.Map<IEnumerable<ResearchLineResource>>(researchLines);
        }

        [HttpGet("{id}")]
        public  IActionResult GetResearchLine(int id)
        {
        var rl =   _context.ResearchLines.SingleOrDefault(r => r.Id == id);
        return Ok(rl);
        }
    }
}