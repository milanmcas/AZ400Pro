using awsapi.Models;
using awsapi.Repository;
using System.ComponentModel.DataAnnotations;

namespace awsapi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> CreateAsync(Customer customer)
        {
            var existingUser = await _customerRepository.GetAsync(customer.Id);
            if (existingUser is not null)
            {
                var message = $"A user with id {customer.Id} already exists";
                throw new ValidationException(message);
            }

            return await _customerRepository.CreateAsync(customer);
        }

        public async Task<Customer?> GetAsync(Guid id)
        {
            return await _customerRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            return await _customerRepository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _customerRepository.DeleteAsync(id);
        }
    }
}
