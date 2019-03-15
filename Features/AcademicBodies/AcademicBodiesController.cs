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
            public  AcademicBodiesController(RefcaDbContext context,
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
            _logger = loggerFactory.CreateLogger<AcademicBodiesController>();
        }

        //GET : /AcademicBodies/List
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> List(){

            var userID= _userManager.GetUserId(User);
            if(userID==null) return View("Error");

            //No estoy muy seguro de que hace esta linea de codigo, seguire investigando
            var academicBodies = await _context.AcademicBodies.ToListAsync();

            return View(academicBodies);
        }


    }
} 