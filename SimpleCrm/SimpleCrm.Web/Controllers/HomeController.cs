using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        {
            var model = new CustomerModel
            {
                Id = 1,
                PhoneNumber = "555-555-1234",
                FirstName = "John",
                LastName = "Doe",
            };

            return View(model);
        }
    }
}
