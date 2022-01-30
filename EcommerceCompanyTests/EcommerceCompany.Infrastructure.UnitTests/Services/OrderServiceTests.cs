using EcommerceCompany.Application.Models;
using EcommerceCompany.Application.Repositories;
using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly OrderService _sut;

        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly ILogger<OrderService> _logger = Substitute.For<ILogger<OrderService>>();
        private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();
        private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();
        private readonly ICustomerStatusServiceFactory _customerStatusServiceFactory = Substitute.For<ICustomerStatusServiceFactory>();
        private readonly ICustomerStatusService _customerStatusService = Substitute.For<ICustomerStatusService>();
        private readonly IDiscountService _discountService = Substitute.For<IDiscountService>();

        public OrderServiceTests()
        {
            _sut = new OrderService(_logger, _orderRepository, _dateTimeService, _customerRepository, _customerStatusServiceFactory, _discountService);
        }

        [Fact]
        public void CalculatePriceAndCreateOrderFromOrderMessage_ShouldCreateOrder_WhenDetailsAreValid()
        {
            // Arrange
            DtoOrder dtoOrder = new DtoOrder
            {
                Amount = 300,
                CustomerId = 1,
                Description = "First order"
            };
            Customer customer = new Customer
            {
                Id = 1,
                CustomerStatus = CustomerType.Regular,
                Name = "Customer"
            };

            _customerRepository.GetCustomerById(dtoOrder.CustomerId).Returns(customer);
            _orderRepository.GetAllFromLast30DaysByCustomerId(dtoOrder.CustomerId).Returns(Array.Empty<Order>());
            _customerStatusServiceFactory.GetServiceByCustomerStatus(customer.CustomerStatus).ReturnsNull();
            _discountService.GetDiscountByCustomerType(CustomerType.Regular).Returns(0);

            // Act
            _sut.CalculatePriceAndCreateOrderFromOrderMessage(dtoOrder);

            // Assert
            _orderRepository.Received(1).Create(Arg.Any<Order>());
        }

        [Fact]
        public void CalculatePriceAndCreateOrderFromOrderMessage_ShouldCreateOrderWithNewlyAppliedDiscount_WhenDetailsAreValidAndCustomerCanBecomeSilver()
        {
            // Arrange
            DtoOrder dtoOrder = new DtoOrder
            {
                Amount = 300,
                CustomerId = 1,
                Description = "First order"
            };
            Customer customer = new Customer
            {
                Id = 1,
                CustomerStatus = CustomerType.Regular,
                Name = "Customer"
            };
            Order[] orders = new Order[]
            {
                new Order { Amount = 200 }
            };
            Customer newCustomer = new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                CustomerStatus = CustomerType.Silver,
                MemberSinceDateUtc = customer.MemberSinceDateUtc
            };

            _customerRepository.GetCustomerById(dtoOrder.CustomerId).Returns(customer);
            _orderRepository.GetAllFromLast30DaysByCustomerId(dtoOrder.CustomerId).Returns(orders);
            _customerStatusServiceFactory.GetServiceByCustomerStatus(customer.CustomerStatus).Returns(_customerStatusService);
            _customerStatusService.CanAdvanceInStatus(customer, 1, 500).Returns(true);
            _customerRepository.Update(Arg.Any<Customer>()).ReturnsForAnyArgs(true);
            _discountService.GetDiscountByCustomerType(CustomerType.Silver).Returns(10);
            _customerRepository.GetCustomerById(newCustomer.Id).Returns(newCustomer);

            // Act
            _sut.CalculatePriceAndCreateOrderFromOrderMessage(dtoOrder);

            // Assert
            _orderRepository.Received(1).Create(Arg.Any<Order>());
        }

        [Fact]
        public void CalculatePriceAndCreateOrderFromOrderMessage_ShouldCreateOrderWithNewlyAppliedDiscount_WhenDetailsAreValidAndCustomerCanBecomeGold()
        {
            // Arrange
            DtoOrder dtoOrder = new DtoOrder
            {
                Amount = 400,
                CustomerId = 1,
                Description = "First order"
            };
            Customer customer = new Customer
            {
                Id = 1,
                CustomerStatus = CustomerType.Silver,
                Name = "Customer"
            };
            Order[] orders = new Order[]
            {
                new Order { Amount = 200 }
            };
            Customer newCustomer = new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                CustomerStatus = CustomerType.Gold,
                MemberSinceDateUtc = customer.MemberSinceDateUtc
            };

            _customerRepository.GetCustomerById(dtoOrder.CustomerId).Returns(customer);
            _orderRepository.GetAllFromLast30DaysByCustomerId(dtoOrder.CustomerId).Returns(orders);
            _customerStatusServiceFactory.GetServiceByCustomerStatus(customer.CustomerStatus).Returns(_customerStatusService);
            _customerStatusService.CanAdvanceInStatus(customer, 1, 500).Returns(true);
            _customerRepository.Update(Arg.Any<Customer>()).ReturnsForAnyArgs(true);
            _discountService.GetDiscountByCustomerType(CustomerType.Gold).Returns(15);
            _customerRepository.GetCustomerById(newCustomer.Id).Returns(newCustomer);

            // Act
            _sut.CalculatePriceAndCreateOrderFromOrderMessage(dtoOrder);

            // Assert
            _orderRepository.Received(1).Create(Arg.Any<Order>());
        }
    }
}
