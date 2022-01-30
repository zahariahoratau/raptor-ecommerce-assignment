using EcommerceCompany.Application.Repositories;
using EcommerceCompany.Infrastructure.Repositories;
using NSubstitute;
using System;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly CustomerRepository _sut;
        private readonly Microsoft.Extensions.Logging.ILogger<CustomerRepository> _logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<CustomerRepository>>();

        public CustomerRepositoryTests()
        {
            _sut = new CustomerRepository(_logger);
        }

        [Fact]
        public void Create_ShouldCreateCustomer_WhenCustomersIsPopulated()
        {
            // Arrange
            
        }
    }
}
