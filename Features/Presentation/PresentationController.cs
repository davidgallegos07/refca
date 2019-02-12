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
using refca.Models.PresentationViewModels;
using AutoMapper;
using refca.Resources;
using System.Collections.Generic;
using refca.Repositories;
using refca.Models.FileViewModel;
using refca.Core;

namespace refca.Features.Presentation
{
    [Authorize]
    public class PresentationController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private IPresentationRepository presentationRepository;
        private readonly IFileProductivityService fileProductivitySvc;


        public PresentationController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment, IMapper mapper, IPresentationRepository presentationRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.presentationRepository = presentationRepository;
            this.fileProductivitySvc = fileProductivitySvc;
        }

        // GET: /Presentation/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Presentation/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var presentationInDb = await context.Presentations.SingleOrDefaultAsync(t => t.Id == id);
            if (presentationInDb == null) return View("NotFound");

            presentationInDb.IsApproved = presentationInDb.IsApproved == true ? presentationInDb.IsApproved = false : presentationInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(PresentationController.Manage));
        }

        // GET: /Presentation/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {
            return View();
        }


        // GET: /Presentation/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var presentationInDb = await presentationRepository.GetPresentation(id);
            if (presentationInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id, ControllerName = "Presentation" });
        }

        // POST: /Presentation/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel presentation)
        {
            var userId = userManager.GetUserId(User);

            var presentationInDb = await presentationRepository.GetPresentation(presentation.Id);
            if (presentationInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(new FileViewModel { Id = presentation.Id, ControllerName = "Presentation" });

            var bucket = $@"/bucket/{userId}/research/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(presentationInDb.PresentationPath);

            presentationInDb.PresentationPath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }








        // GET: /Presentation/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST: /Presentation/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(PresentationViewModel presentation, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(presentation);
            if (!validTeachers(presentation.TeacherIds)) return View("NotFound");

            var newPresentation = mapper.Map<Models.Presentation>(presentation);
            newPresentation.AddedDate = DateTime.Now;
            presentationRepository.Add(newPresentation);

            var writterId = presentation.TeacherIds.SingleOrDefault(i => i == userId);
            if (writterId != null) presentation.TeacherIds.Remove(userId);

            var numOrder = 0;
            context.TeacherPresentations.Add(new TeacherPresentation { TeacherId = userId, PresentationId = newPresentation.Id, Order = ++numOrder, Role = Roles.Writter });
            foreach (var teacher in presentation.TeacherIds)
            {
                context.TeacherPresentations.Add(new TeacherPresentation { TeacherId = teacher, PresentationId = newPresentation.Id, Order = ++numOrder, Role = Roles.Reader });
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(PresentationController.Upload), new { Id = newPresentation.Id });
        }

        // GET: /Presentation/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var presentationInDb = await context.Presentations.FirstOrDefaultAsync(t => t.Id == id);
            if (presentationInDb == null) return View("NotFound");

            var writter = context.TeacherPresentations.FirstOrDefault(a => a.PresentationId == presentationInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var viewModel = mapper.Map<PresentationViewModel>(presentationInDb);

            viewModel.Teachers = context.TeacherPresentations.Where(a => a.PresentationId == presentationInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Presentation/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PresentationViewModel presentation, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);
            var presentationInDb = await presentationRepository.GetPresentation(id);
            var writter = context.TeacherPresentations.FirstOrDefault(a => a.PresentationId == presentationInDb.Id && a.Role == Roles.Writter);

            if (presentationInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");
            if (!validTeachers(presentation.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid) return View(presentation);

            var writterId = presentation.TeacherIds.SingleOrDefault(i => i == writter.TeacherId);
            if (writterId == null)
            {
                var filePath = $@"{environment.WebRootPath}{presentationInDb.PresentationPath}";
                fileProductivitySvc.Remove(filePath);
                presentationRepository.Remove(presentationInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            presentationInDb.UpdatedDate = DateTime.Now;
            mapper.Map<PresentationViewModel, Models.Presentation>(presentation, presentationInDb);

            presentation.TeacherIds.Remove(writterId);

            presentationInDb.TeacherPresentations.Where(t => t.PresentationId == presentationInDb.Id && t.TeacherId != writterId)
            .ToList().ForEach(teacher => presentationInDb.TeacherPresentations.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacherId in presentation.TeacherIds)
            {
                context.TeacherPresentations.Add(new TeacherPresentation { TeacherId = teacherId, PresentationId = presentationInDb.Id, Order = ++numOrder, Role = Roles.Reader });
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // POST: /Presentation/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            var presentationInDb = context.Presentations.FirstOrDefault(t => t.Id == id);
            var writter = context.TeacherPresentations.FirstOrDefault(a => a.PresentationId == presentationInDb.Id && a.Role == Roles.Writter);

            if (presentationInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var filePath = $@"{environment.WebRootPath}{presentationInDb.PresentationPath}";
            fileProductivitySvc.Remove(filePath);
            context.Presentations.Remove(presentationInDb);
            context.SaveChanges();

            return RedirectToPanel();

        }

        #region helpers
        private void ListItems(PresentationViewModel presentation)
        {
            presentation.Teachers = context.TeacherPresentations.Where(b => b.PresentationId == presentation.Id).Select(t => t.Teacher).ToList();
        }

        private bool validTeachers(List<string> teacherIds)
        {
            var existingTeachers = context.Teachers.Select(i => i.Id).ToList();
            var teachers = teacherIds.All(t => existingTeachers.Contains(t));

            return teachers;
        }
        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(PresentationController.Manage));

            return RedirectToAction(nameof(PresentationController.List));
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
                var bucket = $@"/bucket/{author.Id}/presentation/{fileName}";
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

        private IActionResult RedirectToPanel()
        {
            if (User.IsInRole(Roles.Admin)) return RedirectToAction(nameof(PresentationController.Manage));
            return RedirectToAction(nameof(PresentationController.List));
        }
        #endregion
    }
}