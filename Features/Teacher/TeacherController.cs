using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using refca.Data;
using refca.Models.TeacherViewModels;
using refca.Features.Home;
using refca.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using AutoMapper;
using System.Collections.Generic;
using refca.Resources;
using refca.Repositories;
using refca.Models.AccountViewModels;
using System;
using refca.Models;

namespace refca.Features.Teacher
{
    public class TeacherController : Controller
    {
        private RefcaDbContext _context;
        private object _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IAuthorizationService _authorizationService;
        private RoleManager<IdentityRole> _roleManager;
        private IHostingEnvironment _environment;
        private IFileRepository _teacherRepository;

        public TeacherController(RefcaDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuthorizationService authorizationService,
            IHostingEnvironment environment,
            IFileRepository teacherRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _environment = environment;
            _teacherRepository = teacherRepository;

        }

        // GET: /Teacher/List        
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> List()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var teachers = await _context.Teachers
                .Include(k => k.KnowledgeArea)
                .Include(a => a.AcademicBody)
                .Include(l => l.Level)
                .ToListAsync();
            return View(teachers);
        }

        // GET: /Teacher/New
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name");
            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name");
            ViewBag.LevelId = new SelectList(_context.Levels, "Id", "Name");
            return View();
        }

        // POST: /Teacher/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> New(TeacherViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var AcademicBody = _context.AcademicBodies.SingleOrDefault(a => a.Id == model.AcademicBodyId);
            var knowledgeArea = _context.KnowledgeAreas.SingleOrDefault(k => k.Id == model.KnowledgeAreaId);
            var selectedLevel = _context.Levels.SingleOrDefault(l => l.Id == model.LevelId);

            if (AcademicBody == null || knowledgeArea == null || (selectedLevel == null && model.SNI == true))
                return BadRequest();

            var emailTeacher = _context.Teachers.Where(e => e.Email == model.Email).SingleOrDefault();
            var emailAdmin = _context.Users.Where(e => e.Email == model.Email).SingleOrDefault();

            if (emailAdmin != null || emailTeacher != null)
            {
                ListItems(model);
                ModelState.AddModelError(string.Empty, "El email ya esta registrado");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!await _userManager.IsInRoleAsync(user, Roles.Teacher))
                        await _userManager.AddToRoleAsync(user, Roles.Teacher);

                    var teacher = new Models.Teacher
                    {
                        Id = user.Id,
                        Email = model.Email,
                        Name = model.Name,
                        TeacherCode = model.TeacherCode,
                        SNI = model.SNI,
                        IsResearchTeacher = model.IsResearchTeacher,
                        HasProdep = model.HasProdep,
                        KnowledgeAreaId = model.KnowledgeAreaId,
                        AcademicBodyId = model.AcademicBodyId
                    };
                    if (model.SNI == false)
                        teacher.LevelId = null;
                    else
                        teacher.LevelId = model.LevelId;

                    _context.Add(teacher);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(TeacherController.List));
                }
                AddErrors(result);
            }
            ListItems(model);
            return View(model);
        }

        // GET: /Teacher/Edit
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(string id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var teacherInDb = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (teacherInDb == null)
                return View("Error");

            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name");
            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name");
            ViewBag.LevelId = new SelectList(_context.Levels, "Id", "Name");

            var model = new EditTeacherViewModel
            {
                Id = teacherInDb.Id,
                Name = teacherInDb.Name,
                LevelId = teacherInDb.LevelId,
                TeacherCode = teacherInDb.TeacherCode,
                SNI = teacherInDb.SNI,
                IsResearchTeacher = teacherInDb.IsResearchTeacher,
                HasProdep = teacherInDb.HasProdep,
                KnowledgeAreaId = teacherInDb.KnowledgeAreaId,
                AcademicBodyId = teacherInDb.AcademicBodyId
            };

            return View(model);
        }

        // POST: /Teacher/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(EditTeacherViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var teacherInDb = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (teacherInDb == null)
                return View("Error");


            var AcademicBody = _context.AcademicBodies.SingleOrDefault(a => a.Id == model.AcademicBodyId);
            var knowledgeArea = _context.KnowledgeAreas.SingleOrDefault(k => k.Id == model.KnowledgeAreaId);
            var selectedLevel = _context.Levels.SingleOrDefault(l => l.Id == model.LevelId);

            if (AcademicBody == null || knowledgeArea == null || (selectedLevel == null && model.SNI == true))
                return BadRequest();

            if (ModelState.IsValid)
            {
                if (model.SNI == false)
                    model.LevelId = null;

                teacherInDb.Name = model.Name;
                teacherInDb.TeacherCode = model.TeacherCode;
                teacherInDb.HasProdep = model.HasProdep;
                teacherInDb.IsResearchTeacher = model.IsResearchTeacher;
                teacherInDb.LevelId = model.LevelId;
                teacherInDb.SNI = model.SNI;
                teacherInDb.AcademicBodyId = model.AcademicBodyId;
                teacherInDb.KnowledgeAreaId = model.KnowledgeAreaId;

                await _context.SaveChangesAsync();
            }
            else
            {
                ListItemsEdit(model);
                return View(model);
            }
            return RedirectToAction(nameof(TeacherController.List));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = await _userManager.FindByIdAsync(id);
            if (userId == null) return View("Error");

            var teacherInDb = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (teacherInDb == null)
                return View("Error");

            var theses = await _context.Thesis
                .Where(m => m.TeacherTheses.Any(t => t.TeacherId == teacherInDb.Id))
                .Include(tt => tt.TeacherTheses)
                    .ThenInclude(t => t.Teacher)
                .ToListAsync();

            var research = await _context.Research
                .Where(m => m.TeacherResearch.Any(t => t.TeacherId == teacherInDb.Id))
                .Include(tr => tr.TeacherResearch)
                    .ThenInclude(t => t.Teacher)
               .ToListAsync();

            var presentations = await _context.Presentations
                .Where(tb => tb.TeacherPresentations.Any(t => t.TeacherId == teacherInDb.Id))
                .Include(tb => tb.TeacherPresentations)
                    .ThenInclude(t => t.Teacher)
                .ToListAsync();

            var chapterbook = await _context.Chapterbooks
                .Where(m => m.TeacherChapterbooks.Any(t => t.TeacherId == teacherInDb.Id))
                .Include(b => b.TeacherChapterbooks)
                    .ThenInclude(b => b.Teacher)
                .ToListAsync();

            var books = await _context.Books
               .Where(tb => tb.TeacherBooks.Any(t => t.TeacherId == teacherInDb.Id))
               .Include(tb => tb.TeacherBooks)
                   .ThenInclude(t => t.Teacher)
              .ToListAsync();

            var articles = await _context.Articles
               .Where(ta => ta.TeacherArticles.Any(t => t.TeacherId == teacherInDb.Id))
               .Include(ta => ta.TeacherArticles)
                   .ThenInclude(t => t.Teacher)
               .ToListAsync();

            var magazines = await _context.Magazines
                .Where(tb => tb.TeacherMagazines.Any(t => t.TeacherId == teacherInDb.Id))
                .Include(tb => tb.TeacherMagazines)
                    .ThenInclude(t => t.Teacher)
                .ToListAsync();

            _teacherRepository.TeacherThesis(theses, teacherInDb.Id);
            _teacherRepository.TeacherResearch(research, teacherInDb.Id);
            _teacherRepository.TeacherPresentations(presentations, teacherInDb.Id);
            _teacherRepository.TeacherChapterbooks(chapterbook, teacherInDb.Id);
            _teacherRepository.TeacherBooks(books, teacherInDb.Id);
            _teacherRepository.TeacherArticles(articles, teacherInDb.Id);
            _teacherRepository.TeacherMagazines(magazines, teacherInDb.Id);

            var userPath = $@"{_environment.WebRootPath}/bucket/{teacherInDb.Id}/";
            if (Directory.Exists(userPath))
                Directory.Delete(userPath, true);

            _context.Teachers.Remove(teacherInDb);
            await _userManager.DeleteAsync(userId);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(TeacherController.List));
        }

        // GET: /Teacher/Curriculum
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> Curriculum(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var teacherInDb = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == userId);
            if (teacherInDb == null)
                return View("Error");

            var model = new TeacherCurriculumViewModel { CVPath = teacherInDb.CVPath };
            return View(model);
        }

        // POST: /Teacher/Curriculum
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Curriculum(IFormFile CVFile, TeacherCurriculumViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var teacherInDb = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == userId);
            if (teacherInDb == null)
                return View("Error");

            // validating file
            if (!IsValidFile(CVFile))
            {
                return View(model);
            }
            model.CVPath = teacherInDb.CVPath;
            if (CVFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CVFile.FileName);
                var bucket = $@"/bucket/{userId}/curriculum/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";

                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var currentPath = $@"{_environment.WebRootPath}{teacherInDb.CVPath}";
                if (System.IO.File.Exists(currentPath))
                {
                    System.IO.File.Delete(currentPath);
                }

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await CVFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    teacherInDb.CVPath = Path.Combine(bucket, fileName);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                return View(model);
            }
            TempData["StatusMessage"] = "Curriculum actualizado";
            return RedirectToAction(nameof(TeacherController.Curriculum));
        }

        // GET: /Teacher/ResetPassword
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var currentUser = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (currentUser == null)
                return View("Error");

            var model = new ResetPasswordViewModel
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
            };

            return View(model);
        }

        // POST: /Teacher/ResetPassword
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                model.Code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = "Contrase�a reestablecida";
                    return RedirectToAction(nameof(TeacherController.ResetPassword));
                }
                AddErrors(result);
                return View();
            }
            return RedirectToAction(nameof(TeacherController.ResetPassword));
        }

        #region helpers
        private void ListItems(TeacherViewModel teacher)
        {
            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name", teacher.AcademicBodyId);
            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name", teacher.KnowledgeAreaId);
            ViewBag.LevelId = new SelectList(_context.Levels, "Id", "Name", teacher.LevelId);
        }
        private void ListItemsEdit(EditTeacherViewModel model)
        {
            ViewBag.AcademicBodyId = new SelectList(_context.AcademicBodies, "Id", "Name", model.AcademicBodyId);
            ViewBag.KnowledgeAreaId = new SelectList(_context.KnowledgeAreas, "Id", "Name", model.AcademicBodyId);
            ViewBag.LevelId = new SelectList(_context.Levels, "Id", "Name", model.LevelId);
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