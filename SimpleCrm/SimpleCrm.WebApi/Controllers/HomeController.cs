using Microsoft.AspNetCore.Mvc;
using SimpleCrm.WebApi.Models;
using System.Diagnostics;

namespace SimpleCrm.WebApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)] // set action as cacheable
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)] // set action as cacheable
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("contact")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)] // set action as cacheable
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}