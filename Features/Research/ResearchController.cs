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
using refca.Models.ResearchViewModels;
using AutoMapper;
using System.Collections.Generic;
using refca.Repositories;
using refca.Core;
using refca.Models.FileViewModel;
using refca.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace refca.Features.Research
{
    [Authorize]
    public class ResearchController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private IResearchRepository researchRepository;
        private readonly IFileProductivityService fileProductivitySvc;
        public ResearchController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment, IMapper mapper, IResearchRepository researchRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.researchRepository = researchRepository;
            this.fileProductivitySvc = fileProductivitySvc;
        }

        // GET: /Research/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Research/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var researchInDb = await context.Research.SingleOrDefaultAsync(t => t.Id == id);
            if (researchInDb == null) return View("NotFound");

            researchInDb.IsApproved = researchInDb.IsApproved == true ? researchInDb.IsApproved = false : researchInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ResearchController.Manage));
        }

        // GET: /Research/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {
            return View();
        }

        // GET: /Research/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var researchInDb = await researchRepository.GetResearch(id);
            if (researchInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id, ControllerName = "Research" });
        }


        // POST: /Research/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel research)
        {
            var userId = userManager.GetUserId(User);

            var researchInDb = await researchRepository.GetResearch(research.Id);
            if (researchInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(new FileViewModel { Id = research.Id, ControllerName = "Research" });

            var bucket = $@"/bucket/{userId}/research/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(researchInDb.ResearchPath);

            researchInDb.ResearchPath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }


        // GET: /Research/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.KnowledgeAreaId = new SelectList(context.KnowledgeAreas, "Id", "Name");
            ViewBag.AcademicBodyId = new SelectList(context.AcademicBodies, "Id", "Name");

            return View();
        }

        // POST: /Research/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        //IFormFile ResearchFile
        public async Task<IActionResult> New(ResearchViewModel research, string returnUrl = null)

        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(research);
            if (!validTeachers(research.TeacherIds)) return View("NotFound");

            var newResearch = mapper.Map<Models.Research>(research);
            newResearch.AddedDate = DateTime.Now;
            researchRepository.Add(newResearch);

            var writterId = research.TeacherIds.SingleOrDefault(i => i == userId);
            if (writterId != null) research.TeacherIds.Remove(userId);

            var numOrder = 0;
            context.TeacherResearch.Add(new TeacherResearch { TeacherId = userId, ResearchId = newResearch.Id, Order = ++numOrder, Role = Roles.Writter });
            foreach (var teacher in research.TeacherIds)
            {
                context.TeacherResearch.Add(new TeacherResearch { TeacherId = teacher, ResearchId = newResearch.Id, Order = ++numOrder, Role = Roles.Reader });
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ResearchController.Upload), new { Id = newResearch.Id });
        }

        // GET: /Research/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var researchInDb = await context.Research.SingleOrDefaultAsync(t => t.Id == id);
            if (researchInDb == null)
                return View("NotFound");

            ViewBag.KnowledgeAreaId = new SelectList(context.KnowledgeAreas, "Id", "Name", researchInDb.KnowledgeAreaId);
            ViewBag.AcademicBodyId = new SelectList(context.AcademicBodies, "Id", "Name", researchInDb.AcademicBodyId);
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
            viewModel.Teachers = context.TeacherResearch.Where(p => p.ResearchId == researchInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Research/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResearchViewModel research, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);
            var researchInDb = await researchRepository.GetResearch(id);
            var writter = context.TeacherResearch.FirstOrDefault(a => a.ResearchId == researchInDb.Id && a.Role == Roles.Writter);

            if (researchInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");
            if (!validTeachers(research.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid)
            {
                ListItems(research);
                return View(research);
            }

            var writterId = research.TeacherIds.SingleOrDefault(i => i == writter.TeacherId);
            if (writterId == null)
            {
                var filePath = $@"{environment.WebRootPath}{researchInDb.ResearchPath}";
                fileProductivitySvc.Remove(filePath);
                researchRepository.Remove(researchInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            researchInDb.UpdatedDate = DateTime.Now;
            mapper.Map<ResearchViewModel, Models.Research>(research, researchInDb);

            research.TeacherIds.Remove(writterId);

            researchInDb.TeacherResearch.Where(t => t.ResearchId == researchInDb.Id && t.TeacherId != writterId)
            .ToList().ForEach(teacher => researchInDb.TeacherResearch.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacherId in research.TeacherIds)
            {
                context.TeacherResearch.Add(new TeacherResearch { TeacherId = teacherId, ResearchId = researchInDb.Id, Order = ++numOrder, Role = Roles.Reader });
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // POST: /Research/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            var researchInDb = context.Research.FirstOrDefault(t => t.Id == id);
            var writter = context.TeacherResearch.FirstOrDefault(a => a.ResearchId == researchInDb.Id && a.Role == Roles.Writter);


            if (researchInDb == null) return View("NotFound");
            if (User.IsInRole(Roles.Teacher) && userId != writter.TeacherId) return View("AccessDenied");

            var filePath = $@"{environment.WebRootPath}{researchInDb.ResearchPath}";
            fileProductivitySvc.Remove(filePath);
            context.Research.Remove(researchInDb);
            context.SaveChanges();

            return RedirectToPanel();
        }

        #region helpers
        private void ListItems(ResearchViewModel research)
        {
            research.Teachers = context.TeacherResearch.Where(b => b.ResearchId == research.Id).Select(t => t.Teacher).ToList();
            ViewBag.ResearchLineId = new SelectList(context.ResearchLines, "Id", "Name", research.ResearchLineId);
            ViewBag.KnowledgeAreaId = new SelectList(context.KnowledgeAreas, "Id", "Name", research.KnowledgeAreaId);
            ViewBag.AcademicBodyId = new SelectList(context.AcademicBodies, "Id", "Name", research.AcademicBodyId);

        }
        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ResearchController.Manage));

            return RedirectToAction(nameof(ResearchController.List));
        }


        private bool validTeachers(List<string> teacherIds)
        {
            var existingTeachers = context.Teachers.Select(i => i.Id).ToList();
            var teachers = teacherIds.All(t => existingTeachers.Contains(t));

            return teachers;
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

        private IActionResult RedirectToPanel()
        {
            if (User.IsInRole(Roles.Admin)) return RedirectToAction(nameof(ResearchController.Manage));
            return RedirectToAction(nameof(ResearchController.List));
        }
        #endregion
    }
}