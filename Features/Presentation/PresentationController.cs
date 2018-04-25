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

namespace refca.Features.Presentation
{
    [Authorize]
    public class PresentationController : Controller
    {
        private RefcaDbContext _context;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper mapper;

        public PresentationController(RefcaDbContext context, UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
            _userManager = userManager;
            _environment = environment;
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
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var presentationInDb = await _context.Presentations.SingleOrDefaultAsync(t => t.Id == id);
            if (presentationInDb == null)
            {
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                if (presentationInDb.IsApproved == true)
                {
                    presentationInDb.IsApproved = false;
                }
                else
                {
                    presentationInDb.IsApproved = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PresentationController.Manage));
        }

        // GET: /Presentation/List 
        [HttpGet]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> List(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = _userManager.GetUserId(User);

            var presentations = await _context.Presentations
               .Where(tb => tb.TeacherPresentations.Any(t => t.TeacherId == userId))
               .Include(tb => tb.TeacherPresentations)
                   .ThenInclude(t => t.Teacher)
              .OrderBy(d => d.AddedDate)
              .ToListAsync();
            presentations.ForEach(presentation => presentation.TeacherPresentations = presentation.TeacherPresentations.OrderBy(o => o.Order).ToList());

            var results = mapper.Map<IEnumerable<PresentationResource>>(presentations);
            return View(results);
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
        public async Task<IActionResult> New(IFormFile PresentationFile, PresentationViewModel presentation, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            // validating file
            if (!IsValidFile(PresentationFile))
            {
                ListItems(presentation);
                return View();
            }

            presentation.TeacherIds.Add(userId);

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = presentation.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return View("NotFound");

            // getting clean authorList
            var authorList = GetAuthorList(presentation.TeacherIds);
            if (PresentationFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PresentationFile.FileName);
                var bucket = $@"/bucket/{userId}/presentation/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);

                var physicalPath = Path.Combine(userPath, fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await PresentationFile.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var newPresentation = new Models.Presentation
                    {
                        Title = presentation.Title,
                        Congress = presentation.Congress,
                        AddedDate = DateTime.Now,
                        EditionDate = presentation.EditionDate,
                    };

                    newPresentation.PresentationPath = Path.Combine(bucket, fileName);
                    _context.Presentations.Add(newPresentation);
                    await _context.SaveChangesAsync();

                    foreach (var author in authorList)
                    {
                        var teacherPresentation = new TeacherPresentation { TeacherId = author.Id, PresentationId = newPresentation.Id, Order = author.Order };
                        _context.TeacherPresentations.Add(teacherPresentation);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ListItems(presentation);
                    return View(presentation);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El archivo es requerido");
                ListItems(presentation);
                return View(presentation);
            }
            return RedirectToAction(nameof(PresentationController.List));
        }

        // GET: /Presentation/Edit
        [Authorize(Roles = Roles.AdminAndTeacher)]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var presentation = await _context.Presentations.SingleOrDefaultAsync(t => t.Id == id);
            if (presentation == null)
                return View("NotFound");

            var viewModel = new PresentationViewModel
            {
                Id = presentation.Id,
                Congress = presentation.Congress,
                Title = presentation.Title,
                EditionDate = presentation.EditionDate,
            };
            viewModel.Teachers = _context.TeacherPresentations.Where(p => p.PresentationId == presentation.Id)
            .OrderBy(o => o.Order).Select(t => t.Teacher).ToList();

            return View(viewModel);
        }

        // POST: /Presentation/Edit
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile PresentationFile, PresentationViewModel presentation, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var presentationInDb = _context.Presentations.Include(tb => tb.TeacherPresentations).SingleOrDefault(t => t.Id == id);
            if (presentationInDb == null)
                return View("NotFound");

            // Validate null teachersIds
            if (!presentation.TeacherIds.Any())
            {
                _context.Presentations.Remove(presentationInDb);
                await _context.SaveChangesAsync();
                return RedirectToView();
            }

            // validate true teachers
            var existingTeachers = _context.Teachers.Select(i => i.Id).ToList();
            var authorIds = presentation.TeacherIds.All(t => existingTeachers.Contains(t));
            if (authorIds == false)
                return View("NotFound");

            // getting clean authorList
            var authorList = GetAuthorList(presentation.TeacherIds);

            var currentPath = presentationInDb.PresentationPath;

            // if current authors do not have the file, move it to first author
            if (ExistPath(authorList, currentPath))
            {
                var authorId = authorList.Select(i => i.Id).FirstOrDefault();
                currentPath = $@"/bucket/{authorId}/presentation/";
                if (PresentationFile == null)
                {
                    var fileName = Path.GetFileName(presentationInDb.PresentationPath);
                    var sourcePath = $@"{_environment.WebRootPath}{presentationInDb.PresentationPath}";
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
            if (PresentationFile != null)
            {
                // validating file
                if (!IsValidFile(PresentationFile))
                {
                    ListItems(presentation);
                    return View();
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PresentationFile.FileName);
                var bucket = $@"/bucket/{userId}/presentation/";
                var userPath = $@"{_environment.WebRootPath}{bucket}";
                var directory = Path.GetDirectoryName(currentPath);
                var physicalPath = $@"{_environment.WebRootPath}{directory}";

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                var fullPath = Path.Combine(physicalPath, fileName);
                var oldPath = $@"{_environment.WebRootPath}{presentationInDb.PresentationPath}";

                System.IO.File.Delete(oldPath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await PresentationFile.CopyToAsync(stream);
                }

                currentPath = $@"{directory}/{fileName}";
            }
            if (ModelState.IsValid)
            {
                presentationInDb.Title = presentation.Title;
                presentationInDb.Congress = presentation.Congress;
                presentationInDb.UpdatedDate = DateTime.Now;
                presentationInDb.PresentationPath = currentPath;
                presentationInDb.EditionDate = presentation.EditionDate;

                await _context.SaveChangesAsync();

                presentationInDb.TeacherPresentations.Where(t => t.PresentationId == presentationInDb.Id)
                .ToList().ForEach(teacher => presentationInDb.TeacherPresentations.Remove(teacher));
                await _context.SaveChangesAsync();
                foreach (var teacher in authorList)
                {
                    var teacherPresentation = new TeacherPresentation { TeacherId = teacher.Id, PresentationId = presentationInDb.Id, Order = teacher.Order };
                    _context.TeacherPresentations.Add(teacherPresentation);
                }
                await _context.SaveChangesAsync();
            }

            else
            {
                ListItems(presentation);
                return View(presentation);
            }

            return RedirectToView();

        }

        // POST: /Presentation/Delete
        [HttpPost]
        [Authorize(Roles = Roles.AdminAndTeacher)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var presentationInDb = _context.Presentations.SingleOrDefault(t => t.Id == id);
            if (presentationInDb == null)
                return View("NotFound");

            var oldPath = $@"{_environment.WebRootPath}{presentationInDb.PresentationPath}";

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _context.Presentations.Remove(presentationInDb);
            _context.SaveChanges();

            return RedirectToView();

        }

        #region helpers
        private void ListItems(PresentationViewModel presentation)
        {
            presentation.Teachers = _context.TeacherPresentations.Where(b => b.PresentationId == presentation.Id).Select(t => t.Teacher).ToList();
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
        #endregion
    }
}