using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models.Account;

namespace SimpleCrm.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public IActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new CrmUser
                {
                    UserName = model.UserName,
                    DisplayName = model.DisplayName,
                    PasswordHash = model.Password,
                };
                // TODO: create the user record.
                return Content("Success!");
                //return RedirectToAction(nameof(Details), new { id = customer.Id });
            };
            return View();
        }
    }
}
