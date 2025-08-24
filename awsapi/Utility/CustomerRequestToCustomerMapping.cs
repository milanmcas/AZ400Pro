using awsapi.Models;

namespace awsapi.Utility
{
    public static class CustomerRequestToCustomerMapping
    {
        public static Customer ToCustomer(this CustomerRequest customerRequest)
        {
            return new Customer
            {
                Id = Guid.NewGuid(),
                GitHubUsername = customerRequest.GitHubUsername,
                DateOfBirth = customerRequest.DateOfBirth,
                FullName = customerRequest.FullName,
                Email = customerRequest.Email,
                UpdatedAt = DateTime.UtcNow
            };
        }
        public static Customer ToCustomer(this CustomerRequest customerRequest,Guid id)
        {
            return new Customer
            {
                Id = id,
                GitHubUsername = customerRequest.GitHubUsername,
                DateOfBirth = customerRequest.DateOfBirth,
                FullName = customerRequest.FullName,
                Email = customerRequest.Email,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
