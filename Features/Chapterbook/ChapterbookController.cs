using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using refca.Data;
using refca.Models;
using refca.Features.Home;
using refca.Models.Identity;
using refca.Models.ChapterbookViewModels;
using AutoMapper;
using System.Collections.Generic;
using refca.Repositories;
using refca.Core;
using refca.Models.FileViewModel;

namespace refca.Features.Chapterbook
{
    [Authorize]
    public class ChapterbookController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private IChapterbookRepository chapterbookRepository;
        private readonly IFileProductivityService fileProductivitySvc;

        public ChapterbookController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
        IHostingEnvironment environment, IMapper mapper, IChapterbookRepository chapterbookRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.chapterbookRepository = chapterbookRepository;
            this.fileProductivitySvc = fileProductivitySvc;
        }

        // GET: /Chapterbook/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Chapterbook/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var chapterbookInDb = await context.Chapterbooks.SingleOrDefaultAsync(t => t.Id == id);
            if (chapterbookInDb == null) return View("NotFound");

            chapterbookInDb.IsApproved = chapterbookInDb.IsApproved == true ? chapterbookInDb.IsApproved = false : chapterbookInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ChapterbookController.Manage));
        }

        // GET: /Chapterbook/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {
            return View();
        }

        // GET: /Chapterbook/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var chapterbookInDb = await chapterbookRepository.GetChapterbook(id);
            if (chapterbookInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id });
        }
        
        // POST: /Chapterbook/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel chapterbook)
        {
            var userId = userManager.GetUserId(User);

            var chapterbookInDb = await chapterbookRepository.GetChapterbook(chapterbook.Id);
            if (chapterbookInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(chapterbook);

            var bucket = $@"/bucket/{userId}/chapterbook/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(chapterbookInDb.ChapterbookPath);

            chapterbookInDb.ChapterbookPath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }
        
        // GET: /Chapterbook/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST: /Chapterbook/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ChapterbookViewModel chapterbook, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(chapterbook);
            if (!validTeachers(chapterbook.TeacherIds)) return View("NotFound");

            var newChapterbook = mapper.Map<Models.Chapterbook>(chapterbook);
            newChapterbook.Owner = userId;
            chapterbookRepository.Add(newChapterbook);

            var selfAuthor = chapterbook.TeacherIds.FirstOrDefault(a => a == userId);
            if (selfAuthor != null) 
                chapterbook.TeacherIds.Remove(userId);
            
            var numOrder = 0;            
            context.TeacherChapterbooks.Add(new TeacherChapterbook { TeacherId = userId, ChapterbookId = newChapterbook.Id, Order = ++numOrder, Role = Roles.Writter});
            foreach (var teacher in chapterbook.TeacherIds)
            {
                context.TeacherChapterbooks.Add(new TeacherChapterbook { TeacherId = teacher, ChapterbookId = newChapterbook.Id, Order = ++numOrder, Role = Roles.Reader});
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ChapterbookController.Upload), new { Id = newChapterbook.Id });
        }

        // GET: /Chapterbook/Edit
        [HttpGet]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var chapterbookInDb = await context.Chapterbooks.FirstOrDefaultAsync(t => t.Id == id);
            if (chapterbookInDb == null) return View("NotFound");

            var isTeacherChapterbook = context.TeacherChapterbooks.FirstOrDefault(a => a.ChapterbookId == chapterbookInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && isTeacherChapterbook == null) return View("AccessDenied");

            var viewModel = mapper.Map<ChapterbookViewModel>(chapterbookInDb);

            viewModel.Teachers = context.TeacherChapterbooks.Where(a => a.ChapterbookId == chapterbookInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Chapterbook/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChapterbookViewModel chapterbook, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            var chapterbookInDb = await chapterbookRepository.GetChapterbook(id);
            if (chapterbookInDb == null) return View("NotFound");

            var isTeacherChapterbook = context.TeacherChapterbooks
                .FirstOrDefault(a => a.ChapterbookId == chapterbookInDb.Id && a.TeacherId == userId && a.Role == Roles.Writter);
            if (User.IsInRole(Roles.Teacher) && isTeacherChapterbook == null) return View("AccessDenied");
            
            var adminId = chapterbook.TeacherIds.SingleOrDefault(i => i == userId);
            if (!chapterbook.TeacherIds.Any() || adminId == null)
            {
                var filePath = $@"{environment.WebRootPath}{chapterbookInDb.ChapterbookPath}";
                fileProductivitySvc.Remove(filePath);
                chapterbookRepository.Remove(chapterbookInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            if (!validTeachers(chapterbook.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid) return View(chapterbook);

            mapper.Map<ChapterbookViewModel, Models.Chapterbook>(chapterbook, chapterbookInDb);
            chapterbookInDb.UpdatedDate = DateTime.Now;
            
            chapterbook.TeacherIds.Remove(userId);
            
            chapterbookInDb.TeacherChapterbooks.Where(t => t.ChapterbookId == chapterbookInDb.Id && t.TeacherId != userId)
            .ToList().ForEach(teacher => chapterbookInDb.TeacherChapterbooks.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacher in chapterbook.TeacherIds)
            {
                var teacherChapterBooks = new TeacherChapterbook 
                { TeacherId = teacher, ChapterbookId = chapterbookInDb.Id, Order = ++numOrder, Role = Roles.Reader};
                context.TeacherChapterbooks.Add(teacherChapterBooks);
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // POST: /Chapterbook/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var chapterbookInDb = context.Chapterbooks.FirstOrDefault(t => t.Id == id);
            if (chapterbookInDb == null) return View("NotFound");

            var IsTeacherChapterbook = context.TeacherChapterbooks.FirstOrDefault(a => a.ChapterbookId == chapterbookInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && IsTeacherChapterbook == null) return View("NotFound");

            var filePath = $@"{environment.WebRootPath}{chapterbookInDb.ChapterbookPath}";

            fileProductivitySvc.Remove(filePath);
            context.Chapterbooks.Remove(chapterbookInDb);
            context.SaveChanges();

            return RedirectToPanel();
        }

        #region helpers

        private IActionResult RedirectToPanel()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ChapterbookController.Manage));

            return RedirectToAction(nameof(ChapterbookController.List));
        }

        private bool validTeachers(List<string> teacherIds)
        {
            var existingTeachers = context.Teachers.Select(i => i.Id).ToList();
            var teachers = teacherIds.All(t => existingTeachers.Contains(t));

            return teachers;
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #endregion
    }
}