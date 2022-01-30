using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Services
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _sut;

        private readonly ILogger<DiscountService> _logger = Substitute.For<ILogger<DiscountService>>();

        public DiscountServiceTests()
        {
            _sut = new DiscountService(_logger);
        }

        [Fact]
        public void GetDiscountByCustomerType_ShouldReturnValue0_WhenCustomerTypeIsRegular()
        {
            // Arrange
            double discount = 0;
            string customerType = CustomerType.Regular;

            // Act
            var receivedDiscount = _sut.GetDiscountByCustomerType(customerType);

            // Assert
            Assert.Equal(discount, receivedDiscount);
        }

        [Fact]
        public void GetDiscountByCustomerType_ShouldReturnValue10_WhenCustomerTypeIsSilver()
        {
            // Arrange
            double discount = 10;
            string customerType = CustomerType.Silver;

            // Act
            var receivedDiscount = _sut.GetDiscountByCustomerType(customerType);

            // Assert
            Assert.Equal(discount, receivedDiscount);
        }

        [Fact]
        public void GetDiscountByCustomerType_ShouldReturnValue15_WhenCustomerTypeIsGold()
        {
            // Arrange
            double discount = 15;
            string customerType = CustomerType.Gold;

            // Act
            var receivedDiscount = _sut.GetDiscountByCustomerType(customerType);

            // Assert
            Assert.Equal(discount, receivedDiscount);
        }

        [Fact]
        public void GetDiscountByCustomerType_ShouldReturnValue0_WhenCustomerTypeIsInvalid()
        {
            // Arrange
            double discount = 0;
            string customerType = "Default";

            // Act
            var receivedDiscount = _sut.GetDiscountByCustomerType(customerType);

            // Assert
            Assert.Equal(discount, receivedDiscount);
        }
    }
}
