using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using refca.Data;
using refca.Models;
using refca.Features.Home;
using refca.Models.Identity;
using refca.Models.BookViewModels;
using refca.Models.ResearchViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using refca.Dtos;
using AutoMapper;
using System.Collections.Generic;

namespace refca.Features.Research
{
    [Authorize]
    public class ResearchController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;

        public ResearchController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Research/ListAll
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListAll(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var research = await _context.Research
                .Include(tt => tt.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Include(ac => ac.AcademicBody.ConsolidationGrade)
                .Where(p => p.IsApproved == true)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                research.ForEach(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<ResearchWithTeachersDto>>(research);
            return View(results);
        }

        // GET: /Research/ListUnapproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListUnapproved(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var research = await _context.Research
                .Include(tr => tr.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Include(ac => ac.AcademicBody.ConsolidationGrade)
                .Where(p => p.IsApproved == false)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                research.ForEach(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());                

            var results = Mapper.Map<IEnumerable<ResearchWithTeachersDto>>(research);
            return View(results);
        }

        // GET: /Research/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchInDb = await _context.Research.SingleOrDefaultAsync(t => t.Id == id);
            if (researchInDb == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (researchInDb.IsApproved == true)
                {
                    researchInDb.IsApproved = false;
                }
                else
                {
                    researchInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ResearchController.ListUnapproved));
        }

        // GET: /Research/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);

            var research = await _context.Research
                .Where(m => m.TeacherResearch.Any(r => r.TeacherId == userId))
                .Include(tr => tr.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.ResearchLine)
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Include(ac => ac.AcademicBody.ConsolidationGrade)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                research.ForEach(researchi => researchi.TeacherResearch = researchi.TeacherResearch.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<ResearchWithTeachersDto>>(research);
            return View(results);
        }

        // GET: /Research/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name");
            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name");

            return View();
        }

        // POST: /Research/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(IFormFile ResearchFile, ResearchViewModel research, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            // validating file
            if (!IsValidFile(ResearchFile))
            {
                ListItems(research);
                return View();
            }
            research.TeacherIds.Add(userId);
                        
            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = research.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();

            // getting clean authorList
            var authorList = GetAuthorList(research.TeacherIds);

            if (ResearchFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ResearchFile.FileName);
                var bucket = $@"/bucket/{userId}/research/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await ResearchFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var newResearch = new Models.Research
                    {
                        Title = research.Title,
                        Code = research.Code,
                        FinalPeriod = research.FinalPeriod,
                        InitialPeriod = research.InitialPeriod,
                        Sector = research.Sector,
                        ResearchType = research.ResearchType,
                        ResearchDuration = research.ResearchDuration,
                        AddedDate = DateTime.Now,
                        AcademicBodyId = research.AcademicBodyId,
                        KnowledgeAreaId = research.KnowledgeAreaId,
                        ResearchLineId = research.ResearchLineId
                    };
                    newResearch.ResearchPath = Path.Combine(bucket, fileName);
                    _context.Research.Add(newResearch);
                    await _context.SaveChangesAsync();

                    foreach (var author in authorList)
                    {
                        var teacherResearch = new TeacherResearch { TeacherId = author.Id, ResearchId = newResearch.Id, Order = author.Order };
                        _context.TeacherResearch.Add(teacherResearch);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ListItems(research);
                    return View(research);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(research);
                return View(research);
            }
            return RedirectToAction(nameof(ResearchController.List));
        }

        // GET: /Research/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchInDb = await _context.Research.SingleOrDefaultAsync(t => t.Id == id);
            if (researchInDb == null)
                return NotFound();

            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name", researchInDb.KnowledgeAreaId);
            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name", researchInDb.AcademicBodyId);
            var viewModel = new ResearchViewModel
            {
                Title = researchInDb.Title,
                Code = researchInDb.Code,
                FinalPeriod = researchInDb.FinalPeriod,
                InitialPeriod = researchInDb.InitialPeriod,
                Sector = researchInDb.Sector,
                ResearchType = researchInDb.ResearchType,
                ResearchDuration = researchInDb.ResearchDuration,
                AcademicBodyId = researchInDb.AcademicBodyId,
                ResearchLineId = researchInDb.ResearchLineId,
                KnowledgeAreaId = researchInDb.KnowledgeAreaId,

            };
            viewModel.Teachers = _context.TeacherResearch.Where(p => p.ResearchId == researchInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Research/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile ResearchFile, ResearchViewModel research, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchInDb = _context.Research.Include(tr => tr.TeacherResearch).SingleOrDefault(t => t.Id == id);
            if (researchInDb == null)
                return NotFound();

             // Validate null teachersIds
            if(!research.TeacherIds.Any())
            {
                _context.Research.Remove(researchInDb);
                await _context.SaveChangesAsync();
                return RedirectToView();
            }

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = research.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();
            
            // getting clean authorList
            var authorList = GetAuthorList(research.TeacherIds);

            var currentPath = researchInDb.ResearchPath;
            // if current authors do not have the file, move it to first author
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/research/";
                if (ResearchFile == null)
                {
                    var fileName = Path.GetFileName(researchInDb.ResearchPath);
                    var sourcePath = $@"{_environment.WebRootPath}{researchInDb.ResearchPath}";
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

            if (ResearchFile != null)
            {
                // validating file
                if (!IsValidFile(ResearchFile))
                {
                    ListItems(research);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ResearchFile.FileName);
                var bucket = $@"/bucket/{userId}/research/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{researchInDb.ResearchPath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ResearchFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            // saving changes to research
            if (ModelState.IsValid)
            {
                researchInDb.Title = research.Title;
                researchInDb.Code = research.Code;
                researchInDb.FinalPeriod = research.FinalPeriod;
                researchInDb.InitialPeriod = research.InitialPeriod;
                researchInDb.Sector = research.Sector;
                researchInDb.ResearchType = research.ResearchType;
                researchInDb.ResearchDuration = research.ResearchDuration;
                researchInDb.AcademicBodyId = research.AcademicBodyId;
                researchInDb.KnowledgeAreaId = research.KnowledgeAreaId;
                researchInDb.ResearchLineId = research.ResearchLineId;
                researchInDb.UpdatedDate = DateTime.Now;
                researchInDb.ResearchPath = currentPath;
                await _context.SaveChangesAsync();

               researchInDb.TeacherResearch.Where(t => t.ResearchId == researchInDb.Id)
                .ToList().ForEach(teacher => researchInDb.TeacherResearch.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherResearch = new TeacherResearch { TeacherId = teacher.Id, ResearchId = researchInDb.Id, Order = teacher.Order };
                    _context.TeacherResearch.Add(teacherResearch);
                }
                await _context.SaveChangesAsync();
            }

            else
            {
                ListItems(research);
                return View(research);
            }

            return RedirectToView();
        }

        // POST: /Research/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchInDb = _context.Research.SingleOrDefault(t => t.Id == id);
            if (researchInDb == null)
                return NotFound();

            var oldPath = $@"{_environment.WebRootPath}{researchInDb.ResearchPath}";

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _context.Research.Remove(researchInDb);
            _context.SaveChanges();

            return RedirectToView();

        }

        #region helpers
        private void ListItems(ResearchViewModel research)
        {
            research.Teachers = _context.TeacherResearch.Where(b => b.ResearchId == research.Id).Select(t => t.Teacher).ToList();
            ViewBag.ResearchLineId = new SelectList(_context.ResearchLines, "Id", "Name", research.ResearchLineId);
            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name", research.KnowledgeAreaId);
            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name", research.AcademicBodyId);

        }
        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ResearchController.ListAll));

            return RedirectToAction(nameof(ResearchController.List));
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
                var bucket = $@"/bucket/{author.Id}/research/{fileName}";
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
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
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