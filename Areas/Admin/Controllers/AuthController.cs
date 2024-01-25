using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pigga.Areas.Admin.ViewModel;
using Pigga.Models;
using Pigga.Utilities.Enum;

namespace Pigga.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppUser> _roleManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppUser> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect");
                    return View(vm);
                }
            }
            var result=await _userManager.CheckPasswordAsync(user, vm.Password);
            if (result)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect");
                return View(vm);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            return RedirectToAction("Index", "Home", new {area = ""});
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = new()
            {
                UserName = vm.UserName,
                Email = vm.Email,
                Name = vm.Name,
                Surname = vm.Surname
            };


            var result=await _userManager.CreateAsync(user,vm.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                    return View(vm);
                }
            }
            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());

            await _signInManager.SignInAsync(user, isPersistent: false);


            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                if(!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new()
                    {
                        Name = role.ToString()
                    });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        
    }
}
