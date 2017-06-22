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
    public class MagazinesController : Controller
    {
        private ApplicationDbContext _context;

        public MagazinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/magazines
        [HttpGet]
        public IEnumerable<MagazineWithTeachersDto> Get()
        {
            var magazines =  _context.Magazines
               .Include(tp => tp.TeacherMagazines)
                   .ThenInclude(t => t.Teacher)
               .Where(p => p.IsApproved == true)
               .OrderBy(d => d.AddedDate)
               .ToList();
                magazines.ForEach(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            return Mapper.Map<IEnumerable<MagazineWithTeachersDto>>(magazines);
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
                 magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList();

            if (magazine == null)
                return NotFound();

            return Ok(Mapper.Map<MagazineWithTeachersDto>(magazine));
        }
    }
}