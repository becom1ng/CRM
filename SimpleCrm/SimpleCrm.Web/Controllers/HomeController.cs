using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerData _customerData; // (2) a class field to hold the needed dependency

        // (1) the 'constructor' can INJECT data contracts it needs like this
        public HomeController(ICustomerData customerData)
        {
            _customerData = customerData; // (3) store the injected dependency into a class field
        }

        public IActionResult Index()
        {
            // (4) now use the dependency...
            var model = _customerData.GetAll();
            return View(model);
        }
    }
}
