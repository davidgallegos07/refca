using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using refca.Data;
using refca.Models;
using refca.Models.ThesisViewModels;
using refca.Features.Home;
using refca.Models.Identity;
using AutoMapper;
using System.Collections.Generic;
using refca.Resources;

namespace refca.Features.Thesis
{
    [Authorize]
    public class ThesisController : Controller
    {
        private RefcaDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper mapper;

        public ThesisController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Thesis/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage(string returnUrl = null)
        {
            return View();
        }

        // GET: /Thesis/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var thesisInDb = await _context.Thesis.SingleOrDefaultAsync(t => t.Id == id);
            if (thesisInDb == null)
                return View("NotFound");

            if (ModelState.IsValid)
            {
                if (thesisInDb.IsApproved == true)
                {
                    thesisInDb.IsApproved = false;
                }
                else
                {
                    thesisInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ThesisController.Manage));
        }

        // GET: /Thesis/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var theses = await _context.Thesis
                .Where(m => m.TeacherTheses.Any(t => t.TeacherId == userId))
                .Include(tt => tt.TeacherTheses)
                    .ThenInclude(t => t.Teacher)
                .Include(e => e.EducationProgram)
                .Include(r => r.ResearchLine)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
            theses.ForEach(thesis => thesis.TeacherTheses = thesis.TeacherTheses.OrderBy(o => o.Order).ToList());

            var results = mapper.Map<IEnumerable<ThesisResource>>(theses);
            return View(results);
        }

        // GET: /Thesis/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            ViewBag.ResearchLineId = new SelectList(_context.ResearchLines, "Id", "Name");
            ViewBag.EducationProgramId = new SelectList(_context.EducationPrograms, "Id", "Name");
            return View();
        }

        // POST: /Thesis/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(IFormFile ThesisFile, ThesisViewModel thesis, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchLine = _context.ResearchLines.SingleOrDefault(r => r.Id == thesis.ResearchLineId);
            var educationProgram = _context.EducationPrograms.SingleOrDefault(e => e.Id == thesis.EducationProgramId);

            if (researchLine == null || educationProgram == null)
                return BadRequest();

            // validating file
            if (!IsValidFile(ThesisFile))
            {
                ListItems(thesis);
                return View();
            }

            thesis.TeacherIds.Add(userId);

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = thesis.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return View("NotFound");

            // getting clean authorList
            var authorList = GetAuthorList(thesis.TeacherIds);
            if (ThesisFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ThesisFile.FileName);
                var bucket = $@"/bucket/{userId}/thesis/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await ThesisFile.CopyToAsync(stream);
                }


                if (ModelState.IsValid)
                {
                    var newThesis = new Models.Thesis
                    {
                        Title = thesis.Title,
                        StudentName = thesis.StudentName,
                        AddedDate = DateTime.Now,
                        PublishedDate = thesis.PublishedDate,
                        EducationProgramId = thesis.EducationProgramId,
                        ResearchLineId = thesis.ResearchLineId,

                    };
                    newThesis.ThesisPath = Path.Combine(bucket, fileName);
                    _context.Thesis.Add(newThesis);
                    await _context.SaveChangesAsync();

                    foreach (var author in authorList)
                    {
                        var teacherArticle = new TeacherThesis { TeacherId = author.Id, ThesisId = newThesis.Id, Order = author.Order };
                        _context.TeacherTheses.Add(teacherArticle);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ListItems(thesis);
                    return View(thesis);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(thesis);
                return View(thesis);
            }
            return RedirectToAction(nameof(ThesisController.List));
        }

        // GET: /Thesis/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var thesis = await _context.Thesis.SingleOrDefaultAsync(t => t.Id == id);
            if (thesis == null)
                return View("NotFound");

            ViewBag.ResearchLineId = new SelectList(_context.ResearchLines, "Id", "Name", thesis.ResearchLineId);
            ViewBag.EducationProgramId = new SelectList(_context.EducationPrograms, "Id", "Name", thesis.EducationProgramId);
            var viewModel = new ThesisViewModel
            {
                Id = thesis.Id,
                Title = thesis.Title,
                StudentName = thesis.StudentName,
                PublishedDate = thesis.PublishedDate,
                EducationProgramId = thesis.EducationProgramId,
                ResearchLineId = thesis.ResearchLineId

            };
            viewModel.Teachers = _context.TeacherTheses.Where(p => p.ThesisId == thesis.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Thesis/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile ThesisFile, ThesisViewModel thesis, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchLine = _context.ResearchLines.SingleOrDefault(r => r.Id == thesis.ResearchLineId);
            var educationProgram = _context.EducationPrograms.SingleOrDefault(e => e.Id == thesis.EducationProgramId);

            if (researchLine == null || educationProgram == null)
                return BadRequest();


            var thesisInDb = _context.Thesis.Include(e => e.EducationProgram).Include(r => r.ResearchLine).Include(d => d.TeacherTheses)
                            .SingleOrDefault(t => t.Id == id);
            if (thesisInDb == null)
                return View("NotFound");


            // Validate null teachersIds
            if (!thesis.TeacherIds.Any())
            {
                _context.Thesis.Remove(thesisInDb);
                await _context.SaveChangesAsync();
                return RedirectToView();
            }

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = thesis.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return View("NotFound");

            // getting clean authorList
            var authorList = GetAuthorList(thesis.TeacherIds);

            var currentPath = thesisInDb.ThesisPath;
            // if current authors do not have the file, move it to first author
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/thesis/";
                if (ThesisFile == null)
                {
                    var fileName = Path.GetFileName(thesisInDb.ThesisPath);
                    var sourcePath = $@"{_environment.WebRootPath}{thesisInDb.ThesisPath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        currentPath = Path.Combine(currentPath, fileName);
                    }
                }
            }
            // adding new file
            if (ThesisFile != null)
            {
                // validating file
                if (!IsValidFile(ThesisFile))
                {
                    ListItems(thesis);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ThesisFile.FileName);
                var bucket = $@"/bucket/{userId}/thesis/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{thesisInDb.ThesisPath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ThesisFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            // saving changes to thesis
            if (ModelState.IsValid)
            {
                thesisInDb.Title = thesis.Title;
                thesisInDb.StudentName = thesis.StudentName;
                thesisInDb.PublishedDate = thesis.PublishedDate;
                thesisInDb.UpdatedDate = DateTime.Now;
                thesisInDb.EducationProgramId = thesis.EducationProgramId;
                thesisInDb.ResearchLineId = thesis.ResearchLineId;
                thesisInDb.ThesisPath = currentPath;

                thesisInDb.TeacherTheses.Where(t => t.ThesisId == thesisInDb.Id)
                .ToList().ForEach(teacher => thesisInDb.TeacherTheses.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherTheses = new TeacherThesis { TeacherId = teacher.Id, ThesisId = thesisInDb.Id, Order = teacher.Order };
                    _context.TeacherTheses.Add(teacherTheses);
                }
                await _context.SaveChangesAsync();
            }

            else
            {
                ListItems(thesis);
                return View(thesis);
            }

            return RedirectToView();

        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var thesisInDb = _context.Thesis.SingleOrDefault(t => t.Id == id);
            if (thesisInDb == null)
                return View("NotFound");

            var oldPath = $@"{_environment.WebRootPath}{thesisInDb.ThesisPath}";

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _context.Thesis.Remove(thesisInDb);
            _context.SaveChanges();

            return RedirectToView();

        }

        #region helpers

        private void ListItems(ThesisViewModel thesis)
        {
            ViewBag.ResearchLineId = new SelectList(_context.ResearchLines, "Id", "Name", thesis.ResearchLineId);
            ViewBag.EducationProgramId = new SelectList(_context.EducationPrograms, "Id", "Name", thesis.EducationProgramId);
            thesis.Teachers = _context.TeacherTheses.Where(p => p.ThesisId == thesis.Id).Select(t => t.Teacher).ToList();
        }

        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ThesisController.Manage));

            return RedirectToAction(nameof(ThesisController.List));
        }
        public bool IsValidFile(IFormFile file)
        {
            // validate maximum file length
            if (file.Length > 31457280)
            {
                ModelState.AddModelError(string.Empty, "No se permiten archvios mayores de 30MB");
                return false;
            }
            // validate no empty files
            if (file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "No se permiten archvios vacios");
                return false;
            }
            // validate file type 
            if (!file.FileName.EndsWith(".pdf"))
            {
                ModelState.AddModelError(string.Empty, "Solo se permiten archivos con extension .pdf");
                return false;
            }
            return true;
        }
        public bool ExistPath(List<Author> authorIds, string currentPath)
        {
            var fileName = Path.GetFileName(currentPath);
            foreach (var author in authorIds)
            {
                var bucket = $@"/bucket/{author.Id}/thesis/{fileName}";
                if (bucket == currentPath)
                    return false;
            }
            return true;
        }
        public static List<Author> GetAuthorList(List<string> authorIds)
        {
            var order = 1;
            var authorList = new List<Author>();
            var cleanAuthorIds = authorIds.Distinct().ToList();
            foreach (var author in cleanAuthorIds)
            {
                var newAuthor = new Author { Id = author, Order = order++ };
                authorList.Add(newAuthor);
            }
            return authorList;
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

        }
        #endregion
    }
}