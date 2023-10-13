using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleCrm.WebApi.ApiControllers
{


    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;

        public CustomerController(ICustomerData customerData)
        {
            _customerData = customerData;
        }
        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("")] //  ./api/customers
        public IActionResult GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")] //  ./api/customers/:id
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody] Customer model)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody] Customer model)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")] //  ./api/customers/:id
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
