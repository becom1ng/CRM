using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleCrm.WebApi.Models;
using SimpleCrm.WebApi.Validation;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly LinkGenerator _linkGenerator;

        public CustomerController(ICustomerData customerData, LinkGenerator linkGenerator)
        {
            _customerData = customerData;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetCustomers")] //  ./api/customers
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int take = 50)
        {
            if (page < 1) page = 1;
            if (take < 1) take = 1;
            if (take > 100) take = 100;

            var customers = _customerData.GetAll(page - 1, take, "");
            var models = customers.Select(c => new CustomerDisplayViewModel(c));

            var pagination = new PaginationModel
            {
                Previous = page <= 1 ? null : GetCustomerResourceUri(page - 1, take),
                Next = customers.Count < take ? null : GetCustomerResourceUri(page + 1, take)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

            return Ok(models); // 200
        }

        private string GetCustomerResourceUri(int page, int take)
        {
            return _linkGenerator.GetPathByName(HttpContext, "GetCustomers", values: new
            {
                page = page,
                take = take,
            });
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

            var model = new CustomerDisplayViewModel(customer);
            return Ok(model); // 200
        }
        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <returns></returns>
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody] CustomerCreateViewModel model)
        {
            if (model == null) return BadRequest(); // 400
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState); // 0422
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PreferredContactMethod = model.PreferredContactMethod
            };

            _customerData.Add(customer);
            _customerData.Commit();
            return Ok(new CustomerDisplayViewModel(customer)); // 200 // TODO: Change to Create (status 201) once URI generation is covered
        }
        /// <summary>
        /// Updates a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody] CustomerUpdateViewModel model)
        {
            // TODO: Validation check? => return 401 Unauthorized
            
            if (model == null) return BadRequest(); // 400
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var customer = _customerData.Get(id);
            if (customer == null) return NotFound(); // 404

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.EmailAddress = model.EmailAddress;
            customer.OptInNewsletter = model.OptInNewsletter;
            customer.Type = model.Type;
            customer.PreferredContactMethod = model.PreferredContactMethod;

            _customerData.Update(customer);
            _customerData.Commit();
            return Ok(new CustomerDisplayViewModel(customer)); // 200
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
