using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Services
{
    public class CustomerStatusServiceFactoryTests
    {
        private readonly CustomerStatusServiceFactory _sut;

        private readonly ILogger<CustomerStatusServiceFactory> _logger = Substitute.For<ILogger<CustomerStatusServiceFactory>>();
        private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();

        public CustomerStatusServiceFactoryTests()
        {
            _sut = new CustomerStatusServiceFactory(_logger, _dateTimeService);
        }

        [Fact]
        public void GetServiceByCustomerStatus_ShouldReturnRegularCustomerStatusService_WhenProvidedCustomerTypeIsRegular()
        {
            // Arrange
            var customerStatus = CustomerType.Regular;

            // Act
            var receivedCustomerStatusService = _sut.GetServiceByCustomerStatus(customerStatus);

            // Assert
            Assert.NotNull(receivedCustomerStatusService);
            Assert.Equal(typeof(RegularCustomerStatusService), receivedCustomerStatusService!.GetType());
        }

        [Fact]
        public void GetServiceByCustomerStatus_ShouldReturnSilverCustomerStatusService_WhenProvidedCustomerTypeIsSilver()
        {
            // Arrange
            var customerStatus = CustomerType.Silver;

            // Act
            var receivedCustomerStatusService = _sut.GetServiceByCustomerStatus(customerStatus);

            // Assert
            Assert.NotNull(receivedCustomerStatusService);
            Assert.Equal(typeof(SilverCustomerStatusService), receivedCustomerStatusService!.GetType());
        }

        [Fact]
        public void GetServiceByCustomerStatus_ShouldReturnNull_WhenProvidedCustomerTypeWithoutDefinedService()
        {
            // Arrange
            var customerStatus = "Default";

            // Act
            var receivedCustomerStatusService = _sut.GetServiceByCustomerStatus(customerStatus);

            // Assert
            Assert.Null(receivedCustomerStatusService);
        }
    }
}
