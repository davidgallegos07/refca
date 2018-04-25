using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using refca.Data;
using refca.Resources;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class KnowledgeAreasController : Controller
    {
        private readonly RefcaDbContext _context;
        private readonly IMapper mapper;
        public KnowledgeAreasController(RefcaDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;
        }

        // GET: api/knowledgeareas
        [HttpGet]
        public async Task<IEnumerable<KnowledgeAreaResource>> Get()
        {
            var knowledgeAreas = await _context.KnowledgeAreas.ToListAsync();
            return mapper.Map<IEnumerable<KnowledgeAreaResource>>(knowledgeAreas);
        }
    }
}