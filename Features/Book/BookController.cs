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
using refca.Models.BookViewModels;
using AutoMapper;
using System.Collections.Generic;
using refca.Repositories;
using refca.Core;
using refca.Models.FileViewModel;

namespace refca.Features.Book
{
    [Authorize]
    public class BookController : Controller
    {
        private RefcaDbContext context;
        private IHostingEnvironment environment;
        private UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private IBookRepository bookRepository;
        private readonly IFileProductivityService fileProductivitySvc;

        public BookController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
        IHostingEnvironment environment, IMapper mapper, IBookRepository bookRepository, IFileProductivityService fileProductivitySvc)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
            this.bookRepository = bookRepository;
            this.fileProductivitySvc = fileProductivitySvc;
        }

        // GET: /Book/Manage
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Manage()
        {
            return View();
        }

        // GET: /Book/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var bookInDb = await context.Books.SingleOrDefaultAsync(t => t.Id == id);
            if (bookInDb == null) return View("NotFound");

            bookInDb.IsApproved = bookInDb.IsApproved == true ? bookInDb.IsApproved = false : bookInDb.IsApproved = true;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(BookController.Manage));
        }

        // GET: /Book/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult List()
        {
            return View();
        }

        // GET: /Book/Upload
        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var bookInDb = await bookRepository.GetBook(id);
            if (bookInDb == null) return RedirectToPanel();

            return View(new FileViewModel { Id = id, ControllerName = "Book" });
        }
        
        // POST: /Book/Upload
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, FileViewModel book)
        {
            var userId = userManager.GetUserId(User);

            var bookInDb = await bookRepository.GetBook(book.Id);
            if (bookInDb == null) return RedirectToPanel();
            if (!ModelState.IsValid) return View(book);

            var bucket = $@"/bucket/{userId}/book/";
            var uploadFilePath = $@"{environment.WebRootPath}{bucket}";
            var fileName = await fileProductivitySvc.Storage(uploadFilePath, file);

            fileProductivitySvc.Remove(bookInDb.BookPath);

            bookInDb.BookPath = Path.Combine(bucket, fileName);
            await context.SaveChangesAsync();

            return RedirectToPanel();
        }
        
        // GET: /Book/New
        [Authorize(Roles = Roles.Teacher)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST: /Book/New
        [HttpPost]
        [Authorize(Roles = Roles.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(BookViewModel book, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(book);
            if (!validTeachers(book.TeacherIds)) return View("NotFound");

            var newBook = mapper.Map<Models.Book>(book);
            bookRepository.Add(newBook);

            var selfAuthor = book.TeacherIds.FirstOrDefault(a => a == userId);
            if (selfAuthor != null) 
                book.TeacherIds.Remove(userId);
            
            var numOrder = 0;            
            context.TeacherBooks.Add(new TeacherBook { TeacherId = userId, BookId = newBook.Id, Order = ++numOrder, Role = Roles.Writter});
            foreach (var teacher in book.TeacherIds)
            {
                context.TeacherBooks.Add(new TeacherBook { TeacherId = teacher, BookId = newBook.Id, Order = ++numOrder, Role = Roles.Reader});
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(BookController.Upload), new { Id = newBook.Id });
        }

        // GET: /Book/Edit
        [HttpGet]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var bookInDb = await context.Books.FirstOrDefaultAsync(t => t.Id == id);
            if (bookInDb == null) return View("NotFound");

            var isTeacherBook = context.TeacherBooks.FirstOrDefault(a => a.BookId == bookInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && isTeacherBook == null) return View("AccessDenied");

            var viewModel = mapper.Map<BookViewModel>(bookInDb);

            viewModel.Teachers = context.TeacherBooks.Where(a => a.BookId == bookInDb.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Book/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel book, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = userManager.GetUserId(User);

            var bookInDb = await bookRepository.GetBook(id);
            if (bookInDb == null) return View("NotFound");

            var isTeacherBook = context.TeacherBooks
                .FirstOrDefault(a => a.BookId == bookInDb.Id && a.TeacherId == userId && a.Role == Roles.Writter);
            if (User.IsInRole(Roles.Teacher) && isTeacherBook == null) return View("AccessDenied");
            
            var adminId = book.TeacherIds.SingleOrDefault(i => i == userId);
            if (!book.TeacherIds.Any() || adminId == null)
            {
                var filePath = $@"{environment.WebRootPath}{bookInDb.BookPath}";
                fileProductivitySvc.Remove(filePath);
                bookRepository.Remove(bookInDb);
                await context.SaveChangesAsync();
                return RedirectToPanel();
            }

            if (!validTeachers(book.TeacherIds)) return View("AccessDenied");
            if (!ModelState.IsValid) return View(book);

            mapper.Map<BookViewModel, Models.Book>(book, bookInDb);
            bookInDb.UpdatedDate = DateTime.Now;
            
            book.TeacherIds.Remove(userId);
            
            bookInDb.TeacherBooks.Where(t => t.BookId == bookInDb.Id && t.TeacherId != userId)
            .ToList().ForEach(teacher => bookInDb.TeacherBooks.Remove(teacher));
            await context.SaveChangesAsync();

            var numOrder = 1;
            foreach (var teacher in book.TeacherIds)
            {
                var teacherBooks = new TeacherBook 
                { TeacherId = teacher, BookId = bookInDb.Id, Order = ++numOrder, Role = Roles.Reader};
                context.TeacherBooks.Add(teacherBooks);
            }

            await context.SaveChangesAsync();

            return RedirectToPanel();
        }

        // POST: /Book/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var bookInDb = context.Books.FirstOrDefault(t => t.Id == id);
            if (bookInDb == null) return View("NotFound");

            var IsTeacherBook = context.TeacherBooks.FirstOrDefault(a => a.BookId == bookInDb.Id && a.TeacherId == userId);
            if (User.IsInRole(Roles.Teacher) && IsTeacherBook == null) return View("NotFound");

            var filePath = $@"{environment.WebRootPath}{bookInDb.BookPath}";

            fileProductivitySvc.Remove(filePath);
            context.Books.Remove(bookInDb);
            context.SaveChanges();

            return RedirectToPanel();
        }

        #region helpers

        private IActionResult RedirectToPanel()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(BookController.Manage));

            return RedirectToAction(nameof(BookController.List));
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