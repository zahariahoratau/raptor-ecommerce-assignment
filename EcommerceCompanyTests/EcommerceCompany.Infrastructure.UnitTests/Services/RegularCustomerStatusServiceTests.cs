using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Services;
using NSubstitute;
using System;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Services
{
    public class RegularCustomerStatusServiceTests
    {
        private readonly RegularCustomerStatusService _sut;

        private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();

        public RegularCustomerStatusServiceTests()
        {
            _sut = new RegularCustomerStatusService(_dateTimeService);
        }

        [Theory]
        [InlineData(1, 300, 20, 21)]
        [InlineData(2, 400, 20, 21)]
        [InlineData(2, 300, 20, 22)]
        public void CanAdvanceInStatus_ShouldReturnTrue_WhenCustomerCanAdvance(
            int numberOfOrdersFromLast30Days, int valueOfOrdersFromLast30Days, int statusLastChangedDay, int dateTimeNowMinus7DaysDay)
        {
            // Arrange
            Customer customer = new Customer
            {
                Id = 1,
                CustomerStatus = CustomerType.Regular,
                Name = "Customer",
                StatusLastChangedDateUtc = new DateTime(2020, 10, statusLastChangedDay),
            };
            _dateTimeService.DateTimeNowMinus7days.Returns(new DateTime(2020, 10, dateTimeNowMinus7DaysDay));

            // Act
            var result = _sut.CanAdvanceInStatus(customer, numberOfOrdersFromLast30Days, valueOfOrdersFromLast30Days);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 300, 20, 21)]
        [InlineData(1, 200, 20, 21)]
        [InlineData(1, 300, 20, 19)]
        public void CanAdvanceInStatus_ShouldReturnFalse_WhenCustomerCannotAdvance(
            int numberOfOrdersFromLast30Days, int valueOfOrdersFromLast30Days, int statusLastChangedDay, int dateTimeNowMinus7DaysDay)
        {
            // Arrange
            Customer customer = new Customer
            {
                Id = 1,
                CustomerStatus = CustomerType.Regular,
                Name = "Customer",
                StatusLastChangedDateUtc = new DateTime(2020, 10, statusLastChangedDay),
            };
            _dateTimeService.DateTimeNowMinus7days.Returns(new DateTime(2020, 10, dateTimeNowMinus7DaysDay));

            // Act
            var result = _sut.CanAdvanceInStatus(customer, numberOfOrdersFromLast30Days, valueOfOrdersFromLast30Days);

            // Assert
            Assert.False(result);
        }
    }
}
