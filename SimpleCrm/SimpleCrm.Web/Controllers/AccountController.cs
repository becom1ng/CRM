using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models.Account;

namespace SimpleCrm.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CrmUser> userManager;
        private readonly SignInManager<CrmUser> signInManager;

        public AccountController(UserManager<CrmUser> userManager, SignInManager<CrmUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CrmUser
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                    DisplayName = model.DisplayName,
                };
                var createresult = await userManager.CreateAsync(user, model.Password);
                if (createresult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else {
                    foreach (var result in createresult.Errors)
                    {
                        ModelState.AddModelError("", result.Description);
                    }
                }
                //TODO: Failed to create, return error messages
                return View();
            };
            return View();
        }
    }
}
