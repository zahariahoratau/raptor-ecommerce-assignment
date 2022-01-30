using EcommerceCompany.Application.Models;
using EcommerceCompany.Application.Repositories;
using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Helpers.Extensions;
using Microsoft.Extensions.Logging;

namespace EcommerceCompany.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICustomerStatusServiceFactory _customerStatusServiceFactory;
        private readonly IDiscountService _discountService;

        public OrderService(
            ILogger<OrderService> logger,
            IOrderRepository orderRepository,
            IDateTimeService dateTimeService,
            ICustomerRepository customerRepository,
            ICustomerStatusServiceFactory customerStatusServiceFactory,
            IDiscountService discountService)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _dateTimeService = dateTimeService;
            _customerRepository = customerRepository;
            _customerStatusServiceFactory = customerStatusServiceFactory;
            _discountService = discountService;
        }

        public void CalculatePriceAndCreateOrderFromOrderMessage(DtoOrder dtoOrder)
        {
            try
            {
                _logger.LogInformation("Initiating order creation...");

                Customer customer = GetCustomerWithUpdatedStatus(dtoOrder);

                double discountPercentage = GetCustomerDiscountPercentage(customer);

                Order createdOrder = CreateCustomerOrderWithDiscountPercentage(dtoOrder, customer, discountPercentage);

                _logger.LogInformation($"Order created: {createdOrder}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not create order successfully", ex);
            }
        }

        private Customer GetCustomerWithUpdatedStatus(DtoOrder dtoOrder)
        {
            Customer customer = GetCustomerById(dtoOrder.CustomerId);
            Order[] customerOrdersFromLast30Days = GetCustomerOrdersFromLast30DaysById(dtoOrder.CustomerId);

            double last30DaysOrdersValue = GetOrdersTotalValue(customerOrdersFromLast30Days);
            var last30DaysOrdersWithCurrentOrderValue = last30DaysOrdersValue + dtoOrder.Amount;

            var (CanAdvance, NextStatus) = CanCustomerAdvanceInStatus(customer, customerOrdersFromLast30Days, last30DaysOrdersWithCurrentOrderValue);

            if (CanAdvance)
                customer = UpdateCustomerStatus(customer, NextStatus);

            return customer;
        }

        private Order[] GetCustomerOrdersFromLast30DaysById(int customerId)
        {
            return _orderRepository.GetAllFromLast30DaysByCustomerId(customerId);
        }

        private Customer GetCustomerById(int customerId)
        {
            Customer? customer = _customerRepository.GetCustomerById(customerId);

            customer.ThrowIfNull();

            return customer!;
        }

        private static double GetOrdersTotalValue(Order[] customerOrdersFromLast30Days)
        {
            return customerOrdersFromLast30Days
                    .Aggregate(0, (double currentValue, Order nextOrder) => currentValue + nextOrder.Amount);
        }

        private (bool CanAdvance, string NextStatus) CanCustomerAdvanceInStatus(
            Customer customer, Order[] customerOrdersFromLast30Days, double last30DaysOrdersWithCurrentOrderValue)
        {
            var customerStatusService = _customerStatusServiceFactory.GetServiceByCustomerStatus(customer.CustomerStatus);

            (bool, string) canCustomerAdvanceInStatus = (false, string.Empty);

            if (customerStatusService is not null &&
                customerStatusService.CanAdvanceInStatus(customer, customerOrdersFromLast30Days.Length, last30DaysOrdersWithCurrentOrderValue))
                canCustomerAdvanceInStatus = (true, customerStatusService.NextStatus);

            return canCustomerAdvanceInStatus;
        }

        private Order CreateCustomerOrderWithDiscountPercentage(DtoOrder dtoOrder, Customer customer, double discountPercentage)
        {
            var orderToCreate = new Order
            {
                Amount = discountPercentage != 0 ? (dtoOrder.Amount * (100 - discountPercentage) / 100) : dtoOrder.Amount,
                Description = dtoOrder.Description,
                CustomerId = customer!.Id,
                TimestampUtc = _dateTimeService.DateTimeNowUtc
            };

            _orderRepository.Create(orderToCreate);
            return orderToCreate;
        }

        private double GetCustomerDiscountPercentage(Customer customer)
        {
            double discountPercentage = _discountService.GetDiscountByCustomerType(customer.CustomerStatus);

            return discountPercentage;
        }

        private Customer UpdateCustomerStatus(Customer customer, string nextStatus)
        {
            var newCustomer = new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                CustomerStatus = nextStatus,
                MemberSinceDateUtc = customer.MemberSinceDateUtc,
                StatusLastChangedDateUtc = _dateTimeService.DateTimeNowUtc
            };

            _customerRepository.Update(newCustomer);
            newCustomer = _customerRepository.GetCustomerById(newCustomer.Id);

            newCustomer.ThrowIfNull();

            return newCustomer!;
        }
    }
}
