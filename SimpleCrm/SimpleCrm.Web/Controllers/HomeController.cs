using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerData _customerData; // (2) a class field to hold the needed dependency
        private readonly IGreeter _greeter;

        // (1) the 'constructor' can INJECT data contracts it needs like this
        public HomeController(ICustomerData customerData, IGreeter greeter)
        {
            _customerData = customerData; // (3) store the injected dependency into a class field
            _greeter = greeter;
        }

        public IActionResult Index()
        {
            // (4) now use the dependency...
            var model = new HomePageViewModel
            {
                CurrentMessage = _greeter.GetGreeting(),
                Customers = _customerData.GetAll()
            };
            return View(model);
        }
    }
}