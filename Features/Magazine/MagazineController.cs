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
using refca.Dtos;
using System.Collections.Generic;

namespace refca.Features.Magazine
{
    [Authorize]
    public class MagazineController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;

        public MagazineController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Magazine/ListAll
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListAll(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var magazines = await _context.Magazines
               .Include(b => b.TeacherMagazines)
                   .ThenInclude(b => b.Teacher)
               .Where(p => p.IsApproved == true)
              .OrderBy(d => d.AddedDate)
              .ToListAsync();
                magazines.ForEach(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<MagazineWithTeachersDto>>(magazines);
            return View(results);

        }

        // GET: /Magazine/ListUnapproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ListUnapproved(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var magazines = await _context.Magazines
               .Include(b => b.TeacherMagazines)
                   .ThenInclude(b => b.Teacher)
              .Where(p => p.IsApproved == false)
              .OrderBy(d => d.AddedDate)
              .ToListAsync();
                magazines.ForEach(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());
              
            var results = Mapper.Map<IEnumerable<MagazineWithTeachersDto>>(magazines);
            return View(results);
        }

        // GET: /Magazine/IsApproved
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsApproved(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var magazineInDb = await _context.Magazines.SingleOrDefaultAsync(t => t.Id == id);
            if (magazineInDb == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (magazineInDb.IsApproved == true)
                {
                    magazineInDb.IsApproved = false;
                }
                else
                {
                    magazineInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MagazineController.ListUnapproved));
        }

        // GET: /Magazine/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);

            var magazines = await _context.Magazines
               .Where(tb => tb.TeacherMagazines.Any(t => t.TeacherId == userId))
               .Include(tb => tb.TeacherMagazines)
                   .ThenInclude(t => t.Teacher)
              .OrderBy(d => d.AddedDate)
              .ToListAsync();
                magazines.ForEach(magazine => magazine.TeacherMagazines = magazine.TeacherMagazines.OrderBy(o => o.Order).ToList());

            var results = Mapper.Map<IEnumerable<MagazineWithTeachersDto>>(magazines);
            return View(results);
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
        public async Task<IActionResult> New(IFormFile MagazineFile, MagazineViewModel magazine, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            // validating file
            if (!IsValidFile(MagazineFile))
            {
                ListItems(magazine);
                return View();
            }

            magazine.TeacherIds.Add(userId);

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = magazine.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();

            // getting clean authorList
            var authorList = GetAuthorList(magazine.TeacherIds);

            if (MagazineFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MagazineFile.FileName);
                var bucket = $@"/bucket/{userId}/magazine/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await MagazineFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var newMagazine = new Models.Magazine
                    {
                        Title = magazine.Title,
                        Index = magazine.Index,
                        AddedDate = DateTime.Now,
                        EditionDate = magazine.EditionDate,
                        Edition = magazine.Edition,
                        Editor = magazine.Editor,
                        ISSN = magazine.ISSN

                    };
                    newMagazine.MagazinePath = Path.Combine(bucket, fileName);
                    _context.Magazines.Add(newMagazine);
                    await _context.SaveChangesAsync();

                    foreach (var author in authorList)
                    {
                        var teacherArticle = new TeacherMagazine { TeacherId = author.Id, MagazineId = newMagazine.Id, Order = author.Order };
                        _context.TeacherMagazines.Add(teacherArticle);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ListItems(magazine);
                    return View(magazine);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(magazine);
                return View(magazine);
            }
            return RedirectToAction(nameof(MagazineController.List));
        }

        // GET: /Magazine/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var magazine = await _context.Magazines.SingleOrDefaultAsync(t => t.Id == id);
            if (magazine == null)
                return NotFound();

            var viewModel = new MagazineViewModel
            {
                Id = magazine.Id,
                Title = magazine.Title,
                Index = magazine.Index,
                Edition = magazine.Edition,
                EditionDate = magazine.EditionDate,
                Editor = magazine.Editor,
                ISSN = magazine.ISSN,
            };
            viewModel.Teachers = _context.TeacherMagazines.Where(p => p.MagazineId == magazine.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Magazine/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile MagazineFile, MagazineViewModel magazine, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var magazineInDb = _context.Magazines.Include(tb => tb.TeacherMagazines).SingleOrDefault(t => t.Id == id);
            if (magazineInDb == null)
                return NotFound();

            // Validate null teachersIds
            if(!magazine.TeacherIds.Any())
            {
                _context.Magazines.Remove(magazineInDb);
                await _context.SaveChangesAsync();
                return RedirectToView();
            }

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = magazine.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return NotFound();
            
            // getting clean authorList
            var authorList = GetAuthorList(magazine.TeacherIds);

            var currentPath = magazineInDb.MagazinePath;

            // if current authors do not have the file, move it to first author
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/magazine/";
                if (MagazineFile == null)
                {
                    var fileName = Path.GetFileName(magazineInDb.MagazinePath);
                    var sourcePath = $@"{_environment.WebRootPath}{magazineInDb.MagazinePath}";
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
            if (MagazineFile != null)
            {
                // validating file
                if (!IsValidFile(MagazineFile))
                {
                    ListItems(magazine);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MagazineFile.FileName);
                var bucket = $@"/bucket/{userId}/magazine/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{magazineInDb.MagazinePath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await MagazineFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            if (ModelState.IsValid)
            {
                magazineInDb.Title = magazine.Title;
                magazineInDb.Index = magazine.Index;
                magazineInDb.UpdatedDate = DateTime.Now;
                magazineInDb.MagazinePath = currentPath;
                magazineInDb.EditionDate = magazine.EditionDate;
                magazineInDb.Edition = magazine.Edition;
                magazineInDb.Editor = magazine.Editor;
                magazineInDb.ISSN = magazine.ISSN;
                await _context.SaveChangesAsync();

                magazineInDb.TeacherMagazines.Where(t => t.MagazineId == magazineInDb.Id)
                .ToList().ForEach(teacher => magazineInDb.TeacherMagazines.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherMagazines = new TeacherMagazine { TeacherId = teacher.Id, MagazineId = magazineInDb.Id, Order = teacher.Order };
                    _context.TeacherMagazines.Add(teacherMagazines);
                }
                await _context.SaveChangesAsync();
            }

            else
            {
                ListItems(magazine);
                return View(magazine);
            }
            
            return RedirectToView();
        }

        // POST: /Magazine/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");


            var magazineInDb = _context.Magazines.SingleOrDefault(t => t.Id == id);
            if (magazineInDb == null)
                return NotFound();

            var oldPath = $@"{_environment.WebRootPath}{magazineInDb.MagazinePath}";

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _context.Magazines.Remove(magazineInDb);
            _context.SaveChanges();

            return RedirectToView();
            
        }

        #region helpers
        private void ListItems(MagazineViewModel magazine)
        {
            magazine.Teachers = _context.TeacherMagazines.Where(b => b.MagazineId == magazine.Id).Select(t => t.Teacher).ToList();
        }

        private IActionResult RedirectToView()
        {
            if (User.IsInRole(Roles.Admin))
                return RedirectToAction(nameof(MagazineController.ListAll));

            return RedirectToAction(nameof(MagazineController.List));
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
        #endregion
    }
}