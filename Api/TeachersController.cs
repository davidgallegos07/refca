using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using refca.Data;
using refca.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using refca.Dtos;
using System.Threading.Tasks;

namespace refca.Api
{
    [Route("api/[controller]")]
    public class TeachersController : Controller
    {
        private ApplicationDbContext _context;
        public TeachersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: api/teachers
       [HttpGet]
        public IEnumerable<TeacherDto> GetTeachers()
        {
            var teachers = _context.Teachers.ToList();
            return Mapper.Map<IEnumerable<TeacherDto>>(teachers);   
        }

        // GET: api/teachers/search
        [HttpGet("search/{query}")]
        public async Task<IActionResult> GetTeachers(string query)
        {
            var search = await _context.Teachers
                .Where(c => c.Name.Contains(query)).ToListAsync();

            return Ok(Mapper.Map<IEnumerable<TeacherDto>>(search));
        }

        // GET api/teachers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(string id)
        {
            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            return Ok(Mapper.Map<TeacherDto>(teacher));
        }

        // GET api/teachers/{id}/articles/count
        [HttpGet("{id}/articles/count")]
        public async Task<IActionResult> GetNumberOfArticlesByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var articles = await _context.Articles
                .Where(tp => tp.TeacherArticles.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(articles);
        }

        // GET api/teachers/{id}/articles
        [HttpGet("{id}/articles")]
        public async Task<IActionResult> GetArticles(string id)
        { 
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var articles = await _context.Articles
                .Where(tp => tp.TeacherArticles.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .Include(tp => tp.TeacherArticles)
                    .ThenInclude(t => t.Teacher)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                articles.ForEach(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<ArticleWithTeachersDto>>(articles));
        }

        // GET api/teachers/{id}/books/count
        [HttpGet("{id}/books/count")]
        public async Task<IActionResult> GetNumberOfBooksByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var books = await _context.Books
                .Where(tp => tp.TeacherBooks.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(books);
        }


        // GET api/teachers/{id}/books
        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetBooks(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var books = await _context.Books
               .Where(tp => tp.TeacherBooks.Any(t => t.TeacherId == teacher.Id))
               .Where(p => p.IsApproved == true)
               .Include(tp => tp.TeacherBooks)
                   .ThenInclude(t => t.Teacher)
               .OrderBy(d => d.AddedDate)
               .ToListAsync();
                books.ForEach(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<BookWithTeachersDto>>(books));
        }

        // GET api/teachers/{id}/chapterbooks/count
        [HttpGet("{id}/chapterbooks/count")]
        public async Task<IActionResult> GetNumberOfChapterbooksByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var chapterbooks = await _context.Chapterbooks
                .Where(tp => tp.TeacherChapterbooks.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(chapterbooks);
        }
  
        // GET api/teachers/{id}/chapterbooks
        [HttpGet("{id}/chapterbooks")]
        public async Task<IActionResult> GetChapterbooks(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var chapterbooks = await _context.Chapterbooks
               .Where(tp => tp.TeacherChapterbooks.Any(t => t.TeacherId == teacher.Id))
               .Where(p => p.IsApproved == true)
               .Include(tp => tp.TeacherChapterbooks)
                   .ThenInclude(t => t.Teacher)
               .OrderBy(d => d.AddedDate)
               .ToListAsync();
                chapterbooks.ForEach(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<ChapterbookWithTeachersDto>>(chapterbooks));
        }

        // GET api/teachers/{id}/magazines/count
        [HttpGet("{id}/magazines/count")]
        public async Task<IActionResult> GetNumberOfMagazinesByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var magazines = await _context.Magazines
                .Where(tp => tp.TeacherMagazines.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(magazines);
        }

        // GET api/teachers/{id}/magazines
        [HttpGet("{id}/magazines")]
        public async Task<IActionResult> GetMagazines(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var magazines = await _context.Magazines
               .Where(tp => tp.TeacherMagazines.Any(t => t.TeacherId == teacher.Id))
               .Where(p => p.IsApproved == true)
               .Include(tp => tp.TeacherMagazines)
                   .ThenInclude(t => t.Teacher)
               .OrderBy(d => d.AddedDate)
               .ToListAsync();
                magazines.ForEach(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<MagazineWithTeachersDto>>(magazines));
        }

        // GET api/teachers/{id}/presentations/count
        [HttpGet("{id}/presentations/count")]
        public async Task<IActionResult> GetNumberOfPresentationsByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var presentations = await _context.Presentations
                .Where(tp => tp.TeacherPresentations.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(presentations);
        }

        // GET api/teachers/{id}/presentations
        [HttpGet("{id}/presentations")]
        public async Task<IActionResult> GetPresentations(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var presentations = await _context.Presentations
               .Where(tp => tp.TeacherPresentations.Any(t => t.TeacherId == teacher.Id))
               .Where(p => p.IsApproved == true)
               .Include(tp => tp.TeacherPresentations)
                   .ThenInclude(t => t.Teacher)
               .OrderBy(d => d.AddedDate)
               .ToListAsync();
                presentations.ForEach(presentation => presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<PresentationWithTeachersDto>>(presentations));
        }

        // GET api/teachers/{id}/research/count
        [HttpGet("{id}/research/count")]
        public async Task<IActionResult> GetNumberOfResearchByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var research = await _context.Research
                .Where(tp => tp.TeacherResearch.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(research);
        }

        // GET api/teachers/{id}/research
        [HttpGet("{id}/research")]
        public async Task<IActionResult> GetResearch(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var research = await _context.Research
               .Where(tp => tp.TeacherResearch.Any(t => t.TeacherId == teacher.Id))
               .Where(p => p.IsApproved == true)
               .Include(tp => tp.TeacherResearch)
                   .ThenInclude(t => t.Teacher)
               .OrderBy(d => d.AddedDate)
               .ToListAsync();
                research.ForEach(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<ResearchWithTeachersDto>>(research));
        }

        // GET api/teachers/{id}/theses/count
        [HttpGet("{id}/theses/count")]
        public async Task<IActionResult> GetNumberOfThesesByTeacher(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var theses = await _context.Thesis
                .Where(tp => tp.TeacherTheses.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .CountAsync();

            return Ok(theses);
        }

        // GET api/teachers/{id}/theses
        [HttpGet("{id}/theses")]
        public async Task<IActionResult> GetTheses(string id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            var theses = await _context.Thesis
                .Where(tp => tp.TeacherTheses.Any(t => t.TeacherId == teacher.Id))
                .Where(p => p.IsApproved == true)
                .Include(tp => tp.TeacherTheses)
                    .ThenInclude(t => t.Teacher)
                .Include(e => e.EducationProgram)
                .Include(r => r.ResearchLine)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                theses.ForEach(thesis => thesis.TeacherTheses = thesis.TeacherTheses.OrderBy(o => o.Order).ToList());

            return Ok(Mapper.Map<IEnumerable<ThesisWithTeachersDto>>(theses));

        }

    }
}