using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new CustomerModel();
            model.Id = 1;
            model.PhoneNumber = "555-555-1234";
            model.FirstName = "John";
            model.LastName = "Doe";

            return new ObjectResult(model);
        }
    }
}
