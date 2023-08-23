using Microsoft.AspNetCore.Mvc;

namespace SimpleCrm.Web.Controllers
{
    [Route("about")]
    public class AboutController
    {
        [Route("")]
        [Route("phone")]
        public string Phone()
        {
            return "555-867-5307";
        }

        [Route("address")]
        public string Address()
        {
            return "USA";
        }
    }
}
