using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using refca.Data;
using refca.Models;
using refca.Models.AccountViewModels;
using refca.Models.AcademicBodyViewModels;
using refca.Features.Home;
using refca.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace refca.Features.AcademicBodies
{
    public class AcademicBodiesController : Controller


    {
            private readonly RefcaDbContext _context;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IAuthorizationService _authorizationService;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly ILogger _logger;

            private IHostingEnvironment _environment;

            public  AcademicBodiesController(RefcaDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuthorizationService authorizationService,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment
            )
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _logger = loggerFactory.CreateLogger<AcademicBodiesController>();
        }

        //GET : /AcademicBodies/List
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> List(){

             var userId = _userManager.GetUserId(User);
             if(userId == null) return View("Error");

            var academicBodies = await _context.AcademicBodies
                .Include(c=>c.ConsolidationGrade)
                .ToListAsync();
                

            return View(academicBodies);
        }

        //GET : /AcademicBodies/New
        [HttpGet] 
        [Authorize(Roles = Roles.Admin)]
        public IActionResult New (string returnUrl = null)
        {


            ViewData["ReturnUrl"] = returnUrl;

            var userID = _userManager.GetUserId(User);
            if(userID == null) return View("Error");

            ViewBag.ConsolidationGradeId = new SelectList(_context.ConsolidationGrades, "Id", "Name");
            return View();
        }

        //POST : /AcademicBodies/New
        [HttpPost] 
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> New (AcademicBodyViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"]=returnUrl;
            
            var userId = _userManager.GetUserId(User);
            if(userId == null) return View("Error");

            var consolidationGrade = _context.ConsolidationGrades.SingleOrDefault(c => c.Id == model.ConsolidationGradeId);

            if(consolidationGrade == null)
                return BadRequest();
            
            if (ModelState.IsValid)
            {
                  var academicBodies = new Models.AcademicBody
                  {
                       Name = model.Name,
                        PromepCode = model.PromepCode,
                        ConsolidationGradeId = model.ConsolidationGradeId    
                   };

                _context.Add(academicBodies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AcademicBodiesController.List));
            }
            ListItems(model);
            return View(model);
        
        }

        // POST: /AcademicBodies/Delete
        [HttpPost] 
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public  IActionResult Delete ( int id)
        {
            var userId = _userManager.GetUserId(User);
            if(userId == null) return View("Error");
            
            var academicBodiesInDb = _context.AcademicBodies.FirstOrDefault(a => a.Id == id);
            
            if(academicBodiesInDb == null) return View("NotFound");

           _context.AcademicBodies.Remove(academicBodiesInDb);
           _context.SaveChanges();

           return RedirectToAction(nameof(AcademicBodiesController.List));
        }

        //GET : /AcademicBodies/Edit
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> Edit (int id , string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if(userId == null) return View("Error");
          
            var academicBodiesInDb = await _context.AcademicBodies.SingleOrDefaultAsync(a=> a.Id == id);
            if (academicBodiesInDb == null)
                return View("Error");

            ViewBag.ConsolidationGradeId = new SelectList(_context.ConsolidationGrades, "Id", "Name");

            var model = new EditAcademicBodyViewModel
            {
                Id = academicBodiesInDb.Id,
                Name = academicBodiesInDb.Name,
                PromepCode = academicBodiesInDb.PromepCode,
                ConsolidationGradeId = academicBodiesInDb.ConsolidationGradeId
            };

            return View(model);

        }

        // POST: /AcademicBodies/Edit
        [HttpPost] 
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit (EditAcademicBodyViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if(userId == null) return View("Error");

            var academicBodiesInDb = await _context.AcademicBodies.SingleOrDefaultAsync(a=> a.Id == model.Id);
            if (academicBodiesInDb == null)
                return View("Error");

            var consolidationGrade = _context.ConsolidationGrades.SingleOrDefault (c => c.Id==model.ConsolidationGradeId);

            if(consolidationGrade == null)
                return BadRequest();

            if(ModelState.IsValid)
            {
                 academicBodiesInDb.Name = model.Name;
                 academicBodiesInDb.PromepCode = model.PromepCode;
                 academicBodiesInDb.ConsolidationGradeId = model.ConsolidationGradeId;

                 await _context.SaveChangesAsync();
            }
            else
            {
                 ListItemsEdit(model);
                 return View(model);
            }

            return RedirectToAction(nameof(AcademicBodiesController.List));


        }

        #region helpers
        private void ListItems(AcademicBodyViewModel academicBody)
        {
             ViewBag.ConsolidationGradeId = new SelectList(_context.ConsolidationGrades, "Id", "Name", academicBody.ConsolidationGradeId);
        }

        private void ListItemsEdit(EditAcademicBodyViewModel academicBody)
        {
             ViewBag.ConsolidationGradeId = new SelectList(_context.ConsolidationGrades, "Id", "Name", academicBody.ConsolidationGradeId);
        }


        #endregion
    }

} 