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
using refca.Models.ArticleViewModels;
using AutoMapper;
using System.Collections.Generic;
using refca.Repositories;
using refca.Core;
using refca.Models.FileViewModel;

namespace refca.Features.Article
{
    [Authorize]
    public class ArticleController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private IArticleRepository articleRepository;
        private readonly IFileProductivityService fileProductivitySvc;

        public ArticleController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
        IHostingEnvironment environment, IMapper mapper, IArticleRepository articleRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.articleRepository = articleRepository;
            this.fileProductivitySvc = fileProductivitySvc;
        }

        // GET: /Article/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Article/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var articleInDb = await context.Articles.SingleOrDefaultAsync(t => t.Id == id);
            if (articleInDb == null) return View("NotFound");

            articleInDb.IsApproved = articleInDb.IsApproved == true ? articleInDb.IsApproved = false : articleInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ArticleController.Manage));
        }

        // GET: /Article/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {
            return View();
        }

        // GET: /Article/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var articleInDb = await articleRepository.GetArticle(id);
            if (articleInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id });
        }
        
        // POST: /Article/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel article)
        {
            var userId = userManager.GetUserId(User);

            var articleInDb = await articleRepository.GetArticle(article.Id);
            if (articleInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(article);

            var bucket = $@"/bucket/{userId}/article/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(articleInDb.ArticlePath);

            articleInDb.ArticlePath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }
        
        // GET: /Article/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST: /Article/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ArticleViewModel article, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(article);
            if (!validTeachers(article.TeacherIds)) return View("NotFound");

            var newArticle = mapper.Map<Models.Article>(article);
            newArticle.Owner = userId;
            articleRepository.Add(newArticle);

            var selfAuthor = article.TeacherIds.FirstOrDefault(a => a == userId);
            if (selfAuthor != null) 
                article.TeacherIds.Remove(userId);
            
            var numOrder = 0;            
            context.TeacherArticles.Add(new TeacherArticle { TeacherId = userId, ArticleId = newArticle.Id, Order = ++numOrder, Role = Roles.Writter});
            foreach (var teacher in article.TeacherIds)
            {
                context.TeacherArticles.Add(new TeacherArticle { TeacherId = teacher, ArticleId = newArticle.Id, Order = ++numOrder, Role = Roles.Reader});
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(ArticleController.Upload), new { Id = newArticle.Id });
        }

        // GET: /Article/Edit
        [HttpGet]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var articleInDb = await context.Articles.FirstOrDefaultAsync(t => t.Id == id);
            if (articleInDb == null) return View("NotFound");

            var isTeacherArticle = context.TeacherArticles.FirstOrDefault(a => a.ArticleId == articleInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && isTeacherArticle == null) return View("AccessDenied");

            var viewModel = mapper.Map<ArticleViewModel>(articleInDb);

            viewModel.Teachers = context.TeacherArticles.Where(a => a.ArticleId == articleInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Article/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleViewModel article, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            var articleInDb = await articleRepository.GetArticle(id);
            if (articleInDb == null) return View("NotFound");

            var isTeacherArticle = context.TeacherArticles
                .FirstOrDefault(a => a.ArticleId == articleInDb.Id && a.TeacherId == userId && a.Role == Roles.Writter);
            if (User.IsInRole(Roles.Teacher) && isTeacherArticle == null) return View("AccessDenied");
            
            var adminId = article.TeacherIds.SingleOrDefault(i => i == userId);
            if (!article.TeacherIds.Any() || adminId == null)
            {
                var filePath = $@"{environment.WebRootPath}{articleInDb.ArticlePath}";
                fileProductivitySvc.Remove(filePath);
                articleRepository.Remove(articleInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            if (!validTeachers(article.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid) return View(article);

            mapper.Map<ArticleViewModel, Models.Article>(article, articleInDb);
            articleInDb.UpdatedDate = DateTime.Now;
            
            article.TeacherIds.Remove(userId);
            
            articleInDb.TeacherArticles.Where(t => t.ArticleId == articleInDb.Id && t.TeacherId != userId)
            .ToList().ForEach(teacher => articleInDb.TeacherArticles.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacher in article.TeacherIds)
            {
                var teacherArticles = new TeacherArticle 
                { TeacherId = teacher, ArticleId = articleInDb.Id, Order = ++numOrder, Role = Roles.Reader};
                context.TeacherArticles.Add(teacherArticles);
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // POST: /Article/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var articleInDb = context.Articles.FirstOrDefault(t => t.Id == id);
            if (articleInDb == null) return View("NotFound");

            var IsTeacherArticle = context.TeacherArticles.FirstOrDefault(a => a.ArticleId == articleInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && IsTeacherArticle == null) return View("NotFound");

            var filePath = $@"{environment.WebRootPath}{articleInDb.ArticlePath}";

            fileProductivitySvc.Remove(filePath);
            context.Articles.Remove(articleInDb);
            context.SaveChanges();

            return RedirectToPanel();
        }

        #region helpers

        private IActionResult RedirectToPanel()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ArticleController.Manage));

            return RedirectToAction(nameof(ArticleController.List));
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