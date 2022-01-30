using AutoFixture;
using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly OrderRepository _sut;
        private readonly ILogger<OrderRepository> _logger = Substitute.For<ILogger<OrderRepository>>();
        private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();

        public OrderRepositoryTests()
        {
            _sut = new OrderRepository(_logger, _dateTimeService);
        }

        [Fact]
        public void Create_ShouldCreateOrder_WhenOrderObjectIsProvided()
        {
            // Arrange
            var order = new Order();

            // Act
            var result = _sut.Create(order);

            // Assert
            result.Should().BeTrue();
        }
    }
}
