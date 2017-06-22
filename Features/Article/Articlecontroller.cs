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
using refca.Models.ArticleViewModels;
using AutoMapper;
using refca.Dtos;
using System.Collections.Generic;

namespace refca.Features.Article
{
    [Authorize]
    public class ArticleController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;

        public ArticleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Article/ListAll
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListAll(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var articles = await _context.Articles
                .Include(ta => ta.TeacherArticles)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == true)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                articles.ForEach(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<ArticleWithTeachersDto>>(articles);
            return View(results);

        }

        // GET: /Article/ListUnapproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListUnapproved(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var articles = await _context.Articles
                .Include(ta => ta.TeacherArticles)
                    .ThenInclude(t => t.Teacher)
                .Where(p => p.IsApproved == false)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                articles.ForEach(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());
                
            var results = Mapper.Map<IEnumerable<ArticleWithTeachersDto>>(articles);
            return View(results);

        }

        // GET: /Article/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var articleInDb = await _context.Articles.SingleOrDefaultAsync(t => t.Id == id);
            if (articleInDb == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (articleInDb.IsApproved == true)
                {
                    articleInDb.IsApproved = false;
                }
                else
                {
                    articleInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ArticleController.ListUnapproved));
        }

        // GET: /Article/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var articles = await _context.Articles
                .Where(ta => ta.TeacherArticles.Any(t => t.TeacherId == userId))
                .Include(ta => ta.TeacherArticles)
                    .ThenInclude(t => t.Teacher)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
                articles.ForEach(article => article.TeacherArticles = article.TeacherArticles.OrderBy(o => o.Order).ToList());
            
            var results = Mapper.Map<IEnumerable<ArticleWithTeachersDto>>(articles);
            return View(results);
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
        public async Task<IActionResult> New(IFormFile ArticleFile, ArticleViewModel article, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            // validating file
            if (!IsValidFile(ArticleFile))
            {
                ListItems(article);
                return View();
            }
            article.TeacherIds.Add(userId);
                        
            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = article.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();

            // getting clean authorList
            var authorList = GetAuthorList(article.TeacherIds);

            if (ArticleFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ArticleFile.FileName);
                var bucket = $@"/bucket/{userId}/article/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await ArticleFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var newarticle = new Models.Article
                    {
                        Title = article.Title,
                        Magazine = article.Magazine,
                        EditionDate = article.EditionDate,
                        ISSN = article.ISSN,
                        AddedDate = DateTime.Now,
                    };
                    newarticle.ArticlePath = Path.Combine(bucket, fileName);
                    _context.Articles.Add(newarticle);
                    await _context.SaveChangesAsync();

                    foreach (var author in authorList)
                    {
                        var teacherArticle = new TeacherArticle { TeacherId = author.Id, ArticleId = newarticle.Id, Order = author.Order };
                        _context.TeacherArticles.Add(teacherArticle);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ListItems(article);
                    return View(article);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(article);
                return View(article);
            }
            return RedirectToAction(nameof(ArticleController.List));
        }

        // GET: /Article/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var articleInDb = await _context.Articles.SingleOrDefaultAsync(t => t.Id == id);
            if (articleInDb == null)
                return NotFound();

            var viewModel = new ArticleViewModel
            {
                Id = articleInDb.Id,
                Title = articleInDb.Title,
                EditionDate = articleInDb.EditionDate,
                Magazine = articleInDb.Magazine,
                ISSN = articleInDb.ISSN,

            };
            viewModel.Teachers = _context.TeacherArticles.Where(a => a.ArticleId == articleInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Article/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile ArticleFile, ArticleViewModel article, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var articleInDb = await _context.Articles.Include(tb => tb.TeacherArticles).SingleOrDefaultAsync(t => t.Id == id);
            if (articleInDb == null)
                return NotFound();

            // Validate null teachersIds
            if(!article.TeacherIds.Any())
            {
                _context.Articles.Remove(articleInDb);
                await _context.SaveChangesAsync();
                return RedirectToView();
            }

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = article.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();
            
            // getting clean authorList
            var authorList = GetAuthorList(article.TeacherIds);

            var currentPath = articleInDb.ArticlePath;
            // if current authors do not have the file, move it to first author
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/article/";
                if (ArticleFile == null)
                {
                    var fileName = Path.GetFileName(articleInDb.ArticlePath);
                    var sourcePath = $@"{_environment.WebRootPath}{articleInDb.ArticlePath}";
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
            if (ArticleFile != null)
            {
                // validating file
                if (!IsValidFile(ArticleFile))
                {
                    ListItems(article);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ArticleFile.FileName);
                var bucket = $@"/bucket/{userId}/article/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{articleInDb.ArticlePath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ArticleFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            // saving changes to article
            if (ModelState.IsValid)
            {
                articleInDb.Title = article.Title;
                articleInDb.UpdatedDate = DateTime.Now;
                articleInDb.ArticlePath = currentPath;
                articleInDb.Magazine = article.Magazine;
                articleInDb.ISSN = article.ISSN;
                articleInDb.EditionDate = article.EditionDate;
                
                articleInDb.TeacherArticles.Where(t => t.ArticleId == articleInDb.Id)
                .ToList().ForEach(teacher => articleInDb.TeacherArticles.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherArticles = new TeacherArticle { TeacherId = teacher.Id, ArticleId = articleInDb.Id, Order = teacher.Order };
                    _context.TeacherArticles.Add(teacherArticles);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                ListItems(article);
                return View(article);
            }

            return RedirectToView();
        }

        // POST: /Article/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");


            var articleInDb = _context.Articles.SingleOrDefault(t => t.Id == id);
            if (articleInDb == null)
                return NotFound();

            var oldPath = $@"{_environment.WebRootPath}{articleInDb.ArticlePath}";

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _context.Articles.Remove(articleInDb);
            _context.SaveChanges();

            return RedirectToView();
        }

        #region helpers
        private void ListItems(ArticleViewModel article)
        {
            article.Teachers = _context.TeacherArticles.Where(a => a.ArticleId == article.Id).Select(t => t.Teacher).ToList();
        }

        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ArticleController.ListAll));

            return RedirectToAction(nameof(ArticleController.List));
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
                var bucket = $@"/bucket/{author.Id}/article/{fileName}";
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