using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using refca.Models;
using refca.Models.AccountViewModels;
using refca.Services;
using refca.Features.Home;
using refca.Data;
using refca.Models.Identity;
using Microsoft.EntityFrameworkCore;
using refca.Models.TeacherViewModels;
using refca.Features.Teacher;
using refca.Features.Thesis;

namespace refca.Features.Account
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private readonly RefcaDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            RefcaDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _context = context;
        }

        // GET: /Account/Overview
        [HttpGet]
        [Authorize(Roles = (Roles.AdminAndTeacher))]
        public IActionResult Overview(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // GET: /Account/Profile
        [HttpGet]
        [Authorize(Roles = (Roles.Teacher))]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return View("Error");

            var currentUser = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == userId);
            if (currentUser == null)
                return View("Error");

            var model = new TeacherProfileViewModel
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Avatar = currentUser.Avatar,
                WebSite = currentUser.WebSite,
                FacebookProfile = currentUser.FacebookProfile,
                TwitterProfile = currentUser.TwitterProfile,
                Biography = currentUser.Biography
            };
           
            return View(model);
        }

        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Teacher)]
        public async Task<IActionResult> Profile(TeacherProfileViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return View("Error");

            var currentUser = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (currentUser == null)
                return View("Error");

            if (ModelState.IsValid)
            {
                currentUser.Name = model.Name;
                currentUser.Avatar = model.Avatar;
                currentUser.WebSite = model.WebSite;
                currentUser.FacebookProfile = model.FacebookProfile;
                currentUser.TwitterProfile = model.TwitterProfile;
                currentUser.Biography = model.Biography;
                
                await _context.SaveChangesAsync();
            }
            else
            {
                return View(model);
            }
            TempData["StatusMessage"] = "Perfil actualizado";
            return RedirectToAction(nameof(AccountController.Profile));
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            if(_signInManager.IsSignedIn(User))
                return RedirectToAction(nameof(AccountController.Overview));
                
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToAction(nameof(AccountController.Overview));
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de login Invalido.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        
        // GET: /Account/ResetPassword
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult ResetPassword()
        {
            return View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return View("Error");
            
                model.Code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = "Contraseña reestablecida";
                    return RedirectToAction(nameof(AccountController.ResetPassword));
                }
                AddErrors(result);
                return View();
        }

        // GET: /Account/ChangePassword
        [HttpGet]
        [Authorize(Roles = (Roles.AdminAndTeacher))]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [Authorize(Roles = (Roles.AdminAndTeacher))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user == null)
                return View("Error");

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    TempData["StatusMessage"] = "Contraseña actualizada";
                    return RedirectToAction(nameof(AccountController.ChangePassword));
                }
                AddErrors(result);
                return View(model);
        }

        // GET /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

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

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}