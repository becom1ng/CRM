using Microsoft.AspNetCore.Mvc;
using SimpleCrm.WebApi.Models;
using System.Globalization;

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
            var models = customers.Select(c => new CustomerDisplayViewModel
            {
                CustomerId = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                EmailAddress = c.EmailAddress,
                PhoneNumber = c.PhoneNumber,
                Status = Enum.GetName(typeof(CustomerStatus), c.Status),
                PreferredContactMethod = Enum.GetName(typeof(InteractionMethod), c.PreferredContactMethod),
                LastContactDate = c.LastContactDate.Year > 1 ? c.LastContactDate.ToString("s", CultureInfo.InstalledUICulture) : ""
            });
            return Ok(models); //200
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
                return UnprocessableEntity(ModelState); // TODO: Validation error output in a future module
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
                return UnprocessableEntity(ModelState); // TODO: Validation error output in a future module
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
