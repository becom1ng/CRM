using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models.Home;

namespace SimpleCrm.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICustomerData _customerData;

        public HomeController(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                Customers = _customerData.GetAll()
            };
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var cust = _customerData.Get(id);
            if (cust == null) { return RedirectToAction(nameof(Index)); }
            return View(cust);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cust = _customerData.Get(id);
            if (cust == null) { return RedirectToAction(nameof(Index)); }
            var model = new CustomerEditViewModel
            {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                PhoneNumber = cust.PhoneNumber,
                OptInNewsletter = cust.OptInNewsletter,
                Type = cust.Type,
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken] // <- always include with a post action
        public IActionResult Edit(CustomerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cust = _customerData.Get(model.Id);
                {
                    cust.Id = model.Id;
                    cust.FirstName = model.FirstName;
                    cust.LastName = model.LastName;
                    cust.PhoneNumber = model.PhoneNumber;
                    cust.OptInNewsletter = model.OptInNewsletter;
                    cust.Type = model.Type;
                };
                _customerData.Update(cust);
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken] // <- always include with a post action
        public IActionResult Create(CustomerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    OptInNewsletter = model.OptInNewsletter,
                    Type = model.Type
                };
                _customerData.Add(customer);
                return RedirectToAction(nameof(Details), new { id = customer.Id });
            };
            return View();
        }
    }
}