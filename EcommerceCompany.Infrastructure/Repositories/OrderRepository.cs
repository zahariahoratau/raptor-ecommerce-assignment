using EcommerceCompany.Application.Repositories;
using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Helpers.Extensions;
using EcommerceCompany.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace EcommerceCompany.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static DbOrder[] Orders { get; set; } = new DbOrder[]
        {
            new DbOrder
            {
                Amount = 300,
                CustomerId = 1,
                TimestampUtc = DateTime.UtcNow.AddDays(-8),
            },
            new DbOrder
            {
                Amount = 300,
                CustomerId = 1,
                TimestampUtc = DateTime.UtcNow.AddDays(-8),
            }
        };
        private static int NextOrderId { get; set; } = 1;

        private readonly ILogger<OrderRepository> _logger;
        private readonly IDateTimeService _dateTimeService;

        public OrderRepository(ILogger<OrderRepository> logger, IDateTimeService dateTimeService)
        {
            _logger = logger;
            _dateTimeService = dateTimeService;
        }

        public bool Create(Order order)
        {
            try
            {
                Orders = Orders.Append(MapOrderToDbOrderWithNewId(order)).ToArray();
                return true;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the order", ex);
                return false;
            }
        }

        public Order? GetOrderById(int id)
        {
            try
            {
                var dbOrder = Orders.FirstOrDefault((order) => order.Id == id);

                dbOrder.ThrowIfNull();
                
                return MapDbOrderToOrder(dbOrder!);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the order", ex);
                return default;
            }
        }

        public Order[] GetAll()
        {
            try
            {
                return Orders
                    .Select((order) => MapDbOrderToOrder(order))
                    .ToArray();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the orders", ex);
                return Array.Empty<Order>();
            }
        }

        public Order[] GetAllByCustomerId(int customerId)
        {
            try
            {
                return Orders
                    .Where((order) => order.CustomerId == customerId)
                    .Select((order) => MapDbOrderToOrder(order))
                    .ToArray();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the orders", ex);
                return Array.Empty<Order>();
            }
        }

        public Order[] GetAllFromLast30DaysByCustomerId(int customerId)
        {
            try
            {
                return Orders
                    .Where((order) =>
                        order.CustomerId == customerId &&
                        order.TimestampUtc > _dateTimeService.DateTimeNowMinus30Days)
                    .Select((order) => MapDbOrderToOrder(order))
                    .ToArray();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the orders", ex);
                return Array.Empty<Order>();
            }
        }

        public bool ExistsById(int id)
        {
            try
            {
                if (Orders.Any((order) => order.Id == id))
                    return true;

                return false;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the result", ex);
                return false;
            }
        }

        private DbOrder MapOrderToDbOrder(Order order)
        {
            return new DbOrder
            {
                Id = order.Id,
                Amount = order.Amount,
                CustomerId = order.CustomerId,
                Description = order.Description,
                TimestampUtc = order.TimestampUtc
            };
        }

        private DbOrder MapOrderToDbOrderWithNewId(Order order)
        {
            return new DbOrder
            {
                Id = NextOrderId++,
                Amount = order.Amount,
                CustomerId = order.CustomerId,
                Description = order.Description,
                TimestampUtc = order.TimestampUtc
            };
        }

        private Order MapDbOrderToOrder(DbOrder dbOrder)
        {
            return new Order
            {
                Id = dbOrder.Id,
                Amount = dbOrder.Amount,
                CustomerId = dbOrder.CustomerId,
                Description = dbOrder.Description,
                TimestampUtc = dbOrder.TimestampUtc
            };
        }
    }
}
