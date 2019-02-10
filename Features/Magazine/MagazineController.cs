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
using refca.Models.MagazineViewModels;
using AutoMapper;
using refca.Resources;
using refca.Repositories;
using System.Collections.Generic;
using refca.Core;
using refca.Models.FileViewModel;

namespace refca.Features.Magazine
{
    [Authorize]
    public class MagazineController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        private IMagazineRepository magazineRepository;

        private readonly IFileProductivityService fileProductivitySvc;


        public MagazineController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment, IMapper mapper, IMagazineRepository magazineRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.magazineRepository = magazineRepository;
            this.fileProductivitySvc = fileProductivitySvc;
        }

        // GET: /Magazine/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Magazine/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;

            var magazineInDb = await context.Magazines.SingleOrDefaultAsync(t => t.Id == id);
            if (magazineInDb == null) return View("NotFound");

            magazineInDb.IsApproved = magazineInDb.IsApproved == true ? magazineInDb.IsApproved = false : magazineInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(MagazineController.Manage));

        }

        // GET: /Magazine/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {
            return View();

        }

        // GET: /Magazine/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var magazineInDb = await magazineRepository.GetMagazines(id);
            if (magazineInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id, ControllerName = "Magazine" });
        }


        // POST: /magazine/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel magazine)
        {
            var userId = userManager.GetUserId(User);

            var magazineInDb = await magazineRepository.GetMagazines(magazine.Id);
            if (magazineInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(new FileViewModel { Id = magazine.Id, ControllerName = "Magazine" });

            var bucket = $@"/bucket/{userId}/magazine/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(magazineInDb.MagazinePath);

            magazineInDb.MagazinePath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }


        // GET: /Magazine/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST: /Magazine/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(MagazineViewModel magazine, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(magazine);
            if (!validTeachers(magazine.TeacherIds)) return View("NotFound");

            var newMagazine = mapper.Map<Models.Magazine>(magazine);
            newMagazine.AddedDate = DateTime.Now;
            magazineRepository.Add(newMagazine);

            var writterId = magazine.TeacherIds.SingleOrDefault(i => i == userId);
            if (writterId != null) magazine.TeacherIds.Remove(userId);

            var numOrder = 0;
            context.TeacherMagazines.Add(new TeacherMagazine { TeacherId = userId, MagazineId = newMagazine.Id, Order = ++numOrder, Role = Roles.Writter });
            foreach (var teacher in magazine.TeacherIds)
            {
                context.TeacherMagazines.Add(new TeacherMagazine { TeacherId = teacher, MagazineId = newMagazine.Id, Order = ++numOrder, Role = Roles.Reader });
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(MagazineController.Upload), new { Id = newMagazine.Id });
        }

        // GET: /Magazine/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var magazineInDb = await context.Magazines.FirstOrDefaultAsync(t => t.Id == id);
            if (magazineInDb == null) return View("NotFound");

            var writter = context.TeacherMagazines.FirstOrDefault(a => a.MagazineId == magazineInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var viewModel = mapper.Map<MagazineViewModel>(magazineInDb);

            viewModel.Teachers = context.TeacherMagazines.Where(a => a.MagazineId == magazineInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);

        }

        // POST: /Magazine/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MagazineViewModel magazine, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);
            var magazineInDb = await magazineRepository.GetMagazines(id);
            var writter = context.TeacherMagazines.FirstOrDefault(a => a.MagazineId == magazineInDb.Id && a.Role == Roles.Writter);

            if (magazineInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");
            if (!validTeachers(magazine.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid) return View(magazine);

            var writterId = magazine.TeacherIds.SingleOrDefault(i => i == writter.TeacherId);
            if (writterId == null)
            {
                var filePath = $@"{environment.WebRootPath}{magazineInDb.MagazinePath}";
                fileProductivitySvc.Remove(filePath);
                magazineRepository.Remove(magazineInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            magazineInDb.UpdatedDate = DateTime.Now;
            mapper.Map<MagazineViewModel, Models.Magazine>(magazine, magazineInDb);

            magazine.TeacherIds.Remove(writterId);

            magazineInDb.TeacherMagazines.Where(t => t.MagazineId == magazineInDb.Id && t.TeacherId != writterId)
            .ToList().ForEach(teacher => magazineInDb.TeacherMagazines.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacherId in magazine.TeacherIds)
            {
                context.TeacherMagazines.Add(new TeacherMagazine { TeacherId = teacherId, MagazineId = magazineInDb.Id, Order = ++numOrder, Role = Roles.Reader });
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // POST: /Magazine/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            var magazineInDb = context.Magazines.FirstOrDefault(t => t.Id == id);
            var writter = context.TeacherMagazines.FirstOrDefault(a => a.MagazineId == magazineInDb.Id && a.Role == Roles.Writter);

            if (magazineInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var filePath = $@"{environment.WebRootPath}{magazineInDb.MagazinePath}";
            fileProductivitySvc.Remove(filePath);
            context.Magazines.Remove(magazineInDb);
            context.SaveChanges();

            return RedirectToPanel();

        }

        #region helpers
        private void ListItems(MagazineViewModel magazine)
        {
            magazine.Teachers = context.TeacherMagazines.Where(b => b.MagazineId == magazine.Id).Select(t => t.Teacher).ToList();
        }

        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(MagazineController.Manage));

            return RedirectToAction(nameof(MagazineController.List));
        }

        private bool validTeachers(List<string> teacherIds)
        {
            var existingTeachers = context.Teachers.Select(i => i.Id).ToList();
            var teachers = teacherIds.All(t => existingTeachers.Contains(t));

            return teachers;
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
                var bucket = $@"/bucket/{author.Id}/magazine/{fileName}";
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
            if (User.IsInRole(Roles.Admin)) return RedirectToAction(nameof(MagazineController.Manage));
            return RedirectToAction(nameof(MagazineController.List));
        }
        #endregion
    }
}