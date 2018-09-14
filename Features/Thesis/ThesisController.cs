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
using refca.Models.FileViewModel;
using refca.Repositories;
using refca.Core;

namespace refca.Features.Thesis
{
    [Authorize]
    public class ThesisController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private IThesisRepository thesisRepository;
        private readonly IFileProductivityService fileProductivitySvc;
        public ThesisController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment, IMapper mapper, IThesisRepository thesisRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.thesisRepository = thesisRepository;
            this.fileProductivitySvc = fileProductivitySvc;

        }

        // GET: /Thesis/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Thesis/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var thesisInDb = await context.Thesis.SingleOrDefaultAsync(t => t.Id == id);
            if (thesisInDb == null) return View("NotFound");

            thesisInDb.IsApproved = thesisInDb.IsApproved == true ? thesisInDb.IsApproved = false : thesisInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ThesisController.Manage));
        }



        // GET: /Thesis/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {

            return View();
        }


        // GET: /Thesis/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var thesisInDb = await thesisRepository.GetTheses(id);
            if (thesisInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id, ControllerName = "Thesis" });
        }


        // POST: /Thesis/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel thesis)
        {
            var userId = userManager.GetUserId(User);

            var thesisInDb = await thesisRepository.GetTheses(thesis.Id);
            if (thesisInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(new FileViewModel { Id = thesis.Id, ControllerName = "Thesis" });

            var bucket = $@"/bucket/{userId}/thesis/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(thesisInDb.ThesisPath);

            thesisInDb.ThesisPath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // GET: /Thesis/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.ResearchLineId = new SelectList(context.ResearchLines, "Id", "Name");
            ViewBag.EducationProgramId = new SelectList(context.EducationPrograms, "Id", "Name");
            return View();
        }



        // POST: /Thesis/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ThesisViewModel thesis, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);
            ViewBag.ResearchLineId = new SelectList(context.ResearchLines, "Id", "Name");
            ViewBag.EducationProgramId = new SelectList(context.EducationPrograms, "Id", "Name");

            if (!ModelState.IsValid) return View(thesis);
            if (!validTeachers(thesis.TeacherIds)) return View("NotFound");

            var newThesis = mapper.Map<Models.Thesis>(thesis);
            newThesis.AddedDate = DateTime.Now;
            thesisRepository.Add(newThesis);

            var writterId = thesis.TeacherIds.SingleOrDefault(i => i == userId);
            if (writterId != null) thesis.TeacherIds.Remove(userId);

            var numOrder = 0;
            context.TeacherTheses.Add(new TeacherThesis { TeacherId = userId, ThesisId = newThesis.Id, Order = ++numOrder, Role = Roles.Writter });
            foreach (var teacher in thesis.TeacherIds)
            {
                context.TeacherTheses.Add(new TeacherThesis { TeacherId = teacher, ThesisId = newThesis.Id, Order = ++numOrder, Role = Roles.Reader });
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ThesisController.Upload), new { Id = newThesis.Id });
        }



        // GET: /Thesis/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var thesisInDb = await context.Thesis.FirstOrDefaultAsync(t => t.Id == id);
            if (thesisInDb == null) return View("NotFound");

            ViewBag.ResearchLineId = new SelectList(context.ResearchLines, "Id", "Name", thesisInDb.ResearchLineId);
            ViewBag.EducationProgramId = new SelectList(context.EducationPrograms, "Id", "Name", thesisInDb.EducationProgramId);

            var writter = context.TeacherTheses.FirstOrDefault(a => a.ThesisId == thesisInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var viewModel = mapper.Map<ThesisViewModel>(thesisInDb);

            viewModel.Teachers = context.TeacherTheses.Where(a => a.ThesisId == thesisInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Thesis/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ThesisViewModel thesis, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);
            var thesisInDb = await thesisRepository.GetTheses(id);
            var writter = context.TeacherTheses.FirstOrDefault(a => a.ThesisId == thesisInDb.Id && a.Role == Roles.Writter);

            if (thesisInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");
            if (!validTeachers(thesis.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid) return View(thesis);

            var writterId = thesis.TeacherIds.SingleOrDefault(i => i == writter.TeacherId);
            if (writterId == null)
            {
                var filePath = $@"{environment.WebRootPath}{thesisInDb.ThesisPath}";
                fileProductivitySvc.Remove(filePath);
                thesisRepository.Remove(thesisInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            thesisInDb.UpdatedDate = DateTime.Now;
            mapper.Map<ThesisViewModel, Models.Thesis>(thesis, thesisInDb);

            thesis.TeacherIds.Remove(writterId);

            thesisInDb.TeacherTheses.Where(t => t.ThesisId == thesisInDb.Id && t.TeacherId != writterId)
            .ToList().ForEach(teacher => thesisInDb.TeacherTheses.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacherId in thesis.TeacherIds)
            {
                context.TeacherTheses.Add(new TeacherThesis { TeacherId = teacherId, ThesisId = thesisInDb.Id, Order = ++numOrder, Role = Roles.Reader });
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();

        }


        // POST: /Thesis/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            var thesisInDb = context.Thesis.FirstOrDefault(t => t.Id == id);
            var writter = context.TeacherTheses.FirstOrDefault(a => a.ThesisId == thesisInDb.Id && a.Role == Roles.Writter);

            if (thesisInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var filePath = $@"{environment.WebRootPath}{thesisInDb.ThesisPath}";
            fileProductivitySvc.Remove(filePath);
            context.Thesis.Remove(thesisInDb);
            context.SaveChanges();

            return RedirectToPanel();

        }

        #region helpers

        private void ListItems(ThesisViewModel thesis)
        {
            ViewBag.ResearchLineId = new SelectList(context.ResearchLines, "Id", "Name", thesis.ResearchLineId);
            ViewBag.EducationProgramId = new SelectList(context.EducationPrograms, "Id", "Name", thesis.EducationProgramId);
            thesis.Teachers = context.TeacherTheses.Where(p => p.ThesisId == thesis.Id).Select(t => t.Teacher).ToList();
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

        private IActionResult RedirectToPanel()
        {
            if (User.IsInRole(Roles.Admin)) return RedirectToAction(nameof(ThesisController.Manage));
            return RedirectToAction(nameof(ThesisController.List));
        }

        private bool validTeachers(List<string> teacherIds)
        {
            var existingTeachers = context.Teachers.Select(i => i.Id).ToList();
            var teachers = teacherIds.All(t => existingTeachers.Contains(t));

            return teachers;
        }

        #endregion
    }
}