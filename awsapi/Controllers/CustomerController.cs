using awsapi.Models;
using awsapi.Services;
using awsapi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace awsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost("customers")]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            var customer = request.ToCustomer();
            var customerResponse = await _customerService.CreateAsync(customer);

            return CreatedAtAction("Get", new { customer.Id }, customerResponse);
        }

        [HttpGet("customers/{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _customerService.GetAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpPut("customers/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,
             CustomerRequest request)
        {
            var customer = request.ToCustomer(id); 
            var existingCustomer = await _customerService.GetAsync(customer.Id);
            if (existingCustomer is null)
            {
                return NotFound();
            }

            var customerResponse = await _customerService.UpdateAsync(customer);

            return Ok(customerResponse);
        }
        [HttpDelete("customers/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _customerService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
