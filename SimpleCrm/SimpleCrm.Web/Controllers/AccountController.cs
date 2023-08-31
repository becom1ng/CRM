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
                var createResult = await userManager.CreateAsync(user, model.Password);
                if (createResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else {
                    foreach (var result in createResult.Errors)
                    {
                        ModelState.AddModelError("", result.Description);
                    }
                }
                return View();
            };
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        // /Account/Login?returnUrl=....
        // The Http POST version needs to accept a model with email/username and password pair.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (loginResult.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Unable to login.");
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
