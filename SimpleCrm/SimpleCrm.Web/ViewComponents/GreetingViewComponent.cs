using Microsoft.AspNetCore.Mvc;

namespace SimpleCrm.Web.ViewComponents
{
    public class GreetingViewComponent : ViewComponent
    {
        private readonly IGreeter greeter;

        public GreetingViewComponent(IGreeter greeter)
        {
            this.greeter = greeter;
        }

        //public IViewComponentResult Invoke()
        //{
        //    var model = greeter.GetGreeting();
        //    return View("Default", model);
        //}

        public Task<IViewComponentResult> InvokeAsync()
        {
            var model = greeter.GetGreeting();
            return Task.FromResult<IViewComponentResult>(View("Default", model));
        }
    }
}