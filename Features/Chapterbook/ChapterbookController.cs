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
using refca.Models.ChapterbookViewModels;
using System.Collections.Generic;
using refca.Dtos;
using AutoMapper;

namespace refca.Features.Chapterbook
{
    [Authorize]
    public class ChapterbookController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;

        public ChapterbookController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Chapterbook/ListAll
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListAll(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var chapterbooks = await _context.Chapterbooks
                 .Include(b => b.TeacherChapterbooks)
                     .ThenInclude(b => b.Teacher)
                 .Where(p => p.IsApproved == true)
                 .OrderBy(d => d.AddedDate)
                 .ToListAsync();
                chapterbooks.ForEach(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<ChapterbookWithTeachersDto>>(chapterbooks);
            return View(results);
        }

        // GET: /Chapterbook/ListUnapproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListUnapproved(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var chapterbooks = await _context.Chapterbooks
                 .Include(b => b.TeacherChapterbooks)
                     .ThenInclude(b => b.Teacher)
                 .Where(p => p.IsApproved == false)
                 .OrderBy(d => d.AddedDate)
                 .ToListAsync();
                chapterbooks.ForEach(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<ChapterbookWithTeachersDto>>(chapterbooks);
            return View(results);
        }

        // GET: /Chapterbook/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var chapterbookInDb = await _context.Chapterbooks.SingleOrDefaultAsync(t => t.Id == id);
            if (chapterbookInDb == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (chapterbookInDb.IsApproved == true)
                {
                    chapterbookInDb.IsApproved = false;
                }
                else
                {
                    chapterbookInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ChapterbookController.ListUnapproved));
        }

        // GET: /Chapterbook/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var chapterbooks = await _context.Chapterbooks
                 .Where(m => m.TeacherChapterbooks.Any(r => r.TeacherId == userId))
                 .Include(b => b.TeacherChapterbooks)
                     .ThenInclude(b => b.Teacher)
                 .OrderBy(d => d.AddedDate)
                 .ToListAsync();
                chapterbooks.ForEach(chapterbook => chapterbook.TeacherChapterbooks = chapterbook.TeacherChapterbooks.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<ChapterbookWithTeachersDto>>(chapterbooks);
            return View(results);
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
        public async Task<IActionResult> New(IFormFile ChapterbookFile, ChapterbookViewModel chapterbook, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            // validating file
            if (!IsValidFile(ChapterbookFile))
            {
                ListItems(chapterbook);
                return View();
            }
            chapterbook.TeacherIds.Add(userId);

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = chapterbook.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();

            // getting clean authorList
            var authorList = GetAuthorList(chapterbook.TeacherIds);

            if (ChapterbookFile != null)
            {
               var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ChapterbookFile.FileName);
                var bucket = $@"/bucket/{userId}/chapterbook/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await ChapterbookFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var newChapterbook = new Models.Chapterbook
                    {
                        Title = chapterbook.Title,
                        BookTitle = chapterbook.BookTitle,
                        PublishedDate = chapterbook.PublishedDate,
                        AddedDate = DateTime.Now,
                        ISBN = chapterbook.ISBN,
                        Editorial = chapterbook.Editorial

                    };
                    newChapterbook.ChapterbookPath = Path.Combine(bucket, fileName);
                    _context.Chapterbooks.Add(newChapterbook);
                    await _context.SaveChangesAsync();

                    foreach (var author in authorList)
                    {
                        var teacherChapterbook = new TeacherChapterbook { TeacherId = author.Id, ChapterbookId = newChapterbook.Id, Order = author.Order };
                        _context.TeacherChapterbooks.Add(teacherChapterbook);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ListItems(chapterbook);
                    return View(chapterbook);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(chapterbook);
                return View(chapterbook);
            }
            return RedirectToAction(nameof(ChapterbookController.List));
        }

        // GET: /Chapterbook/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var chapterbookInDb = await _context.Chapterbooks.SingleOrDefaultAsync(t => t.Id == id);
            if (chapterbookInDb == null)
            {
                return NotFound();
            }

            var viewModel = new ChapterbookViewModel
            {
                Id = chapterbookInDb.Id,
                Title = chapterbookInDb.Title,
                BookTitle = chapterbookInDb.BookTitle,
                PublishedDate = chapterbookInDb.PublishedDate,
                ISBN = chapterbookInDb.ISBN,
                Editorial = chapterbookInDb.Editorial,

            };
            viewModel.Teachers = _context.TeacherChapterbooks.Where(p => p.ChapterbookId == chapterbookInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Chapterbook/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile ChapterbookFile, ChapterbookViewModel chapterbook, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var chapterbookInDb = _context.Chapterbooks.Include(tb => tb.TeacherChapterbooks).SingleOrDefault(t => t.Id == id);
            if (chapterbookInDb == null)
                return NotFound();
            
            // Validate null teachersIds
            if(!chapterbook.TeacherIds.Any())
            {
                _context.Chapterbooks.Remove(chapterbookInDb);
                await _context.SaveChangesAsync();
                return RedirectToView();
            }

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = chapterbook.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();
            
            // getting clean authorList
            var authorList = GetAuthorList(chapterbook.TeacherIds);

            var currentPath = chapterbookInDb.ChapterbookPath;
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/chapterbook/";
                if (ChapterbookFile == null)
                {
                    var fileName = Path.GetFileName(chapterbookInDb.ChapterbookPath);
                    var sourcePath = $@"{_environment.WebRootPath}{chapterbookInDb.ChapterbookPath}";
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
            if (ChapterbookFile != null)
            {
                // validating file
                if (!IsValidFile(ChapterbookFile))
                {
                    ListItems(chapterbook);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ChapterbookFile.FileName);
                var bucket = $@"/bucket/{userId}/chapterbook/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{chapterbookInDb.ChapterbookPath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ChapterbookFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            // saving changes to chapterbook
            if (ModelState.IsValid)
            {
                chapterbookInDb.Title = chapterbook.Title;
                chapterbookInDb.UpdatedDate = DateTime.Now;
                chapterbookInDb.PublishedDate = chapterbook.PublishedDate;
                chapterbookInDb.ChapterbookPath = currentPath;
                chapterbookInDb.ISBN = chapterbook.ISBN;
                chapterbookInDb.Editorial = chapterbook.Editorial;

                chapterbookInDb.TeacherChapterbooks.Where(t => t.ChapterbookId == chapterbookInDb.Id)
                .ToList().ForEach(teacher => chapterbookInDb.TeacherChapterbooks.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherChapterbook = new TeacherChapterbook { TeacherId = teacher.Id, ChapterbookId = chapterbookInDb.Id, Order = teacher.Order };
                    _context.TeacherChapterbooks.Add(teacherChapterbook);
                }
                await _context.SaveChangesAsync();
  
            }
            else
            {
                ListItems(chapterbook);
                return View(chapterbook);
            }

            return RedirectToView();
        }

        // POST: /Chapterbook/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");


            var chapterbookInDb = _context.Chapterbooks.SingleOrDefault(t => t.Id == id);
            if (chapterbookInDb == null)
                return NotFound();

            var oldPath = $@"{_environment.WebRootPath}{chapterbookInDb.ChapterbookPath}";

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _context.Chapterbooks.Remove(chapterbookInDb);
            _context.SaveChanges();

            return RedirectToView();
        }

        #region helpers
        private void ListItems(ChapterbookViewModel chapterbook)
        {
            chapterbook.Teachers = _context.TeacherChapterbooks.Where(b => b.ChapterbookId == chapterbook.Id).Select(t => t.Teacher).ToList();
        }
        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(ChapterbookController.ListAll));

            return RedirectToAction(nameof(ChapterbookController.List));
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
                var bucket = $@"/bucket/{author.Id}/chapterbook/{fileName}";
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