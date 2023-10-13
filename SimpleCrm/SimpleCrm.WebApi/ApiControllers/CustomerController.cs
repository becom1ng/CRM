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
            var customers = _customerData.GetAll(0, 50, "");
            return Ok(customers); //200
        }
        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")] //  ./api/customers/:id
        public IActionResult Get(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound(); // 404
            }
            return Ok(customer); // 200
        }
        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <returns></returns>
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody] Customer model)
        {
            if (model == null)
            {
                return BadRequest(); // 400
            }
            if (!ModelState.IsValid)
            {
                // TODO: generate error once validation is implemented;
            }

            _customerData.Add(model); // TODO: fix later as this is bad
            _customerData.Commit();
            return Ok(model); // 200 // TODO: Change to Create (status 201) once URI generation is covered
        }
        /// <summary>
        /// Updates a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody] Customer model)
        {
            // TODO: Validation check? => return 401 Unauthorized
            
            if (model == null)
            {
                return BadRequest(); // 400
            }

            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound(); // 404
            }

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.EmailAddress = model.EmailAddress;
            customer.OptInNewsletter = model.OptInNewsletter;
            customer.Type = model.Type;
            customer.InteractionMethod = model.InteractionMethod;

            _customerData.Update(model);
            _customerData.Commit();
            return Ok(model); // 200
        }
        /// <summary>
        /// Deletes a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")] //  ./api/customers/:id
        public IActionResult Delete(int id)
        {
            // TODO: Validation check? => return 401 Unauthorized

            var customer = _customerData.Get(id);
            if (customer != null)
            {
                _customerData.Delete(customer);
                _customerData.Commit();
            }
            return NoContent(); // 204, same result for deletion or null
        }
    }
}
