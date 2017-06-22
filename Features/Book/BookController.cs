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
using AutoMapper;
using refca.Dtos;
using System.Collections.Generic;

namespace refca.Features.Book
{
    [Authorize]
    public class BookController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;

        public BookController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Book/ListAll
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListAll(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var books = await _context.Books
                .Include(b => b.TeacherBooks)
                    .ThenInclude(b => b.Teacher)
                .Where(p => p.IsApproved == true)
                .OrderBy(d => d.AddedDate)
                .ToListAsync();
            books.ForEach(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<BookWithTeachersDto>>(books);
            return View(results);

        }

        // GET: /Book/ListUnapproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListUnapproved(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var books = await _context.Books
               .Include(b => b.TeacherBooks)
                   .ThenInclude(b => b.Teacher)
              .Where(p => p.IsApproved == false)
              .OrderBy(d => d.AddedDate)
              .ToListAsync();
            books.ForEach(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<BookWithTeachersDto>>(books);
            return View(results);
        }

        // GET: /Book/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var bookInDb = await _context.Books.SingleOrDefaultAsync(t => t.Id == id);
            if (bookInDb == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (bookInDb.IsApproved == true)
                {
                    bookInDb.IsApproved = false;
                }
                else
                {
                    bookInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(BookController.ListUnapproved));
        }

        // GET: /Book/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);

            var books = await _context.Books
               .Where(tb => tb.TeacherBooks.Any(t => t.TeacherId == userId))
               .Include(tb => tb.TeacherBooks)
                   .ThenInclude(t => t.Teacher)
              .OrderBy(d => d.AddedDate)
              .ToListAsync();
            books.ForEach(book => book.TeacherBooks = book.TeacherBooks.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<BookWithTeachersDto>>(books);
            return View(results);
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
        public async Task<IActionResult> New(IFormFile BookFile, BookViewModel book, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            // validating file
            if (!IsValidFile(BookFile))
            {
                ListItems(book);
                return View();
            }
            book.TeacherIds.Add(userId);
                        
            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = book.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();

            // getting clean authorList
            var authorList = GetAuthorList(book.TeacherIds);

            if (BookFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(BookFile.FileName);
                var bucket = $@"/bucket/{userId}/book/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await BookFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var newBook = new Models.Book
                    {
                        Title = book.Title,
                        Abstract = book.Abstract,
                        AddedDate = DateTime.Now,
                        EditionDate = book.EditionDate,
                        Year = book.Year,
                        ISBN = book.ISBN,
                        Edition = book.Edition,
                        Editorial = book.Editorial,
                        PrintLength = book.PrintLength,
                        Genre = book.Genre

                    };
                    newBook.BookPath = Path.Combine(bucket, fileName);
                    _context.Books.Add(newBook);
                    await _context.SaveChangesAsync();
                    
                    foreach (var author in authorList)
                    {
                        var teacherBook = new TeacherBook { TeacherId = author.Id, BookId = newBook.Id, Order = author.Order };
                        _context.TeacherBooks.Add(teacherBook);
                    }
                    await _context.SaveChangesAsync();

                }
                else
                {
                    ListItems(book);
                    return View(book);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(book);
                return View(book);
            }
            return RedirectToAction(nameof(BookController.List));
        }

        // GET: /Book/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var book = await _context.Books.SingleOrDefaultAsync(t => t.Id == id);
            if (book == null)
                return NotFound();

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Abstract = book.Abstract,
                EditionDate = book.EditionDate,
                Year = book.Year,
                ISBN = book.ISBN,
                Edition = book.Edition,
                Editorial = book.Editorial,
                PrintLength = book.PrintLength,
                Genre = book.Genre

            };
            viewModel.Teachers = _context.TeacherBooks.Where(p => p.BookId == book.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Book/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile BookFile, BookViewModel book, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var bookInDb = _context.Books.Include(tb => tb.TeacherBooks).SingleOrDefault(t => t.Id == id);
            if (bookInDb == null)
                return NotFound();
            
            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = book.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();
            
            // getting clean authorList
            var authorList = GetAuthorList(book.TeacherIds);

            var currentPath = bookInDb.BookPath;
            
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/book/";
                if (BookFile == null)
                {
                    var fileName = Path.GetFileName(bookInDb.BookPath);
                    var sourcePath = $@"{_environment.WebRootPath}{bookInDb.BookPath}";
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
            if (BookFile != null)
            {
                // validating file
                if (!IsValidFile(BookFile))
                {
                    ListItems(book);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(BookFile.FileName);
                var bucket = $@"/bucket/{userId}/book/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{bookInDb.BookPath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await BookFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            if (ModelState.IsValid)
            {
                bookInDb.Title = book.Title;
                bookInDb.Abstract = book.Abstract;
                bookInDb.UpdatedDate = DateTime.Now;
                bookInDb.BookPath = currentPath;
                bookInDb.EditionDate = book.EditionDate;
                bookInDb.Year = book.Year;
                bookInDb.ISBN = book.ISBN;
                bookInDb.Edition = book.Edition;
                bookInDb.Editorial = book.Editorial;
                bookInDb.PrintLength = book.PrintLength;
                bookInDb.Genre = book.Genre;

                await _context.SaveChangesAsync();

                bookInDb.TeacherBooks.Where(t => t.BookId == bookInDb.Id)
                .ToList().ForEach(teacher => bookInDb.TeacherBooks.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherBooks = new TeacherBook { TeacherId = teacher.Id, BookId = bookInDb.Id, Order = teacher.Order };
                    _context.TeacherBooks.Add(teacherBooks);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                ListItems(book);
                return View(book);
            }
            
            return RedirectToView();
        }

        // POST: /Book/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");


            var bookInDb = _context.Books.SingleOrDefault(t => t.Id == id);
            if (bookInDb == null)
                return NotFound();

            var oldPath = $@"{_environment.WebRootPath}{bookInDb.BookPath}";

            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);

            _context.Books.Remove(bookInDb);
            _context.SaveChanges();

            return RedirectToView();

        }

        #region helpers
        private void ListItems(BookViewModel book)
        {
            book.Teachers = _context.TeacherBooks.Where(b => b.BookId == book.Id).Select(t => t.Teacher).ToList();
        }
        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(BookController.ListAll));

            return RedirectToAction(nameof(BookController.List));
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
                var bucket = $@"/bucket/{author.Id}/book/{fileName}";
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