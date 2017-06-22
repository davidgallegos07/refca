using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using refca.Data;
using refca.Models;
using refca.Models.TeacherViewModels;
using refca.Features.Home;
using refca.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using refca.Models.AdminViewModels;

namespace refca.Features.Admin
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public AdminController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuthorizationService authorizationService,
            ILoggerFactory loggerFactory
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _logger = loggerFactory.CreateLogger<AdminController>();
        }

        // GET: /Admin/Administrators

        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> Administrators()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

             var adminRole =  await _context.Roles.SingleOrDefaultAsync(m => m.Name == Roles.Admin);
             var ownerRole =  await _context.Roles.SingleOrDefaultAsync(m => m.Name == Roles.Owner);
             var users =  _context.Users.Where(m => m.Roles.Any(r => r.RoleId == adminRole.Id || r.RoleId == ownerRole.Id))
                 .Include(r => r.Roles);

            return View(users);
        }

        // GET: /Admin/New
        [HttpGet]
        [Authorize(Roles = Roles.Owner)]
        public IActionResult New(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Admin/New
        [HttpPost]
        [Authorize(Roles = Roles.Owner)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(AdminViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!await _userManager.IsInRoleAsync(user, Roles.Admin))
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Admin);
                    }

                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(AdminController.Administrators));
                }
                AddErrors(result);
            }

            return View(model);
        }

        // POST: /Admin/Delete
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return View("Error");

            if (await _userManager.IsInRoleAsync(user, Roles.Admin) && !await _userManager.IsInRoleAsync(user, Roles.Owner))
            {
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Administrators));
        }

        #region helpers

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