using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleCrm.WebApi.Filters;
using SimpleCrm.WebApi.Models;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/customers")]
    [Authorize(Policy = "ApiUser")] // policy created in startup.cs
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerData customerData, LinkGenerator linkGenerator, ILogger<CustomerController> logger)
        {
            _customerData = customerData;
            _linkGenerator = linkGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetCustomers")] //  ./api/customers
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)] // set action as cacheable
        public IActionResult GetCustomers([FromQuery] CustomerListParameters resourceParameters)
        {
            if (resourceParameters.Page < 1) resourceParameters.Page = 1;
            if (resourceParameters.Take < 1) resourceParameters.Take = 1;
            if (resourceParameters.Take > 100) resourceParameters.Take = 100;

            var customers = _customerData.GetAll(resourceParameters);
            var models = customers.Select(c => new CustomerDisplayViewModel(c));

            var pagination = new PaginationModel
            {
                Previous = CreateCustomersResourceUri(resourceParameters, -1),
                // If all customers fit on one page, there is no next page
                Next = customers.Count < resourceParameters.Take ? null : CreateCustomersResourceUri(resourceParameters, 1)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));
            // TODO? Add ETag

            _logger.LogInformation("GetCustomers executed successfully.");
            return Ok(models); // 200
        }

        private string CreateCustomersResourceUri(CustomerListParameters listParameters, int pageAdjust)
        {
            if (listParameters.Page + pageAdjust <= 0)
                return null; // if current page is first page, there is no previous page url

            return _linkGenerator.GetPathByName(HttpContext, "GetCustomers", values: new
            {
                take = listParameters.Take,
                page = listParameters.Page + pageAdjust, // +1 or -1 from current page of data
                orderBy = listParameters.OrderBy,
                lastName = listParameters.LastName,
                firstName = listParameters.FirstName,
                type = listParameters.Type,
                status = listParameters.Status,
                term = listParameters.Term,
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
            Response.Headers.Add("ETag", $"{customer.LastModified}");

            var model = new CustomerDisplayViewModel(customer);

            _logger.LogInformation($"Get customer by id executed successfully. id:{id}");
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
            _logger.LogInformation($"A new customer was created. id:{customer.Id}", customer);
            return Ok(new CustomerDisplayViewModel(customer)); // 200 // TODO? Change to Create (status 201) once URI generation is covered
        }

        /// <summary>
        /// Updates a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody] CustomerUpdateViewModel model)
        {
            if (model == null) return BadRequest(); // 400
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var customer = _customerData.Get(id);
            if (customer == null) return NotFound(); // 404

            string ifMatch = Request.Headers["If-Match"];
            if (ifMatch != customer.LastModified.ToString())
            {
                return StatusCode(422, "The customer data has been changed since it was requested. Please reload and try again."); // 422
            }

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.EmailAddress = model.EmailAddress;
            customer.OptInNewsletter = model.OptInNewsletter;
            customer.Type = model.Type;
            customer.PreferredContactMethod = model.PreferredContactMethod;
            customer.LastModified = DateTime.UtcNow;

            _customerData.Update(customer);
            _customerData.Commit();
            _logger.LogInformation($"Customer id:{id} was updated.");
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
            var customer = _customerData.Get(id);
            if (customer != null)
            {
                _logger.LogInformation($"Customer id:{id} was DELETED.", customer);
                _customerData.Delete(customer);
                _customerData.Commit();
            }
            return NoContent(); // 204, same result for deletion or null
        }
    }
}
