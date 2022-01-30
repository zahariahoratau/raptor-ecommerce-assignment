using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EcommerceCompany.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDictionary<string, double> _customerTypeDiscounts = new Dictionary<string, double>();

        private readonly ILogger<DiscountService> _logger;

        public DiscountService(ILogger<DiscountService> logger)
        {
            _logger = logger;

            _customerTypeDiscounts.Add(CustomerType.Regular, 0);
            _customerTypeDiscounts.Add(CustomerType.Silver, 10);
            _customerTypeDiscounts.Add(CustomerType.Gold, 15);
        }

        public double GetDiscountByCustomerType(string customerType)
        {
            try
            {
                var result = _customerTypeDiscounts.TryGetValue(customerType, out double discount);

                if (result)
                {
                    _logger.LogInformation($"Got {discount}% discount on orders for {customerType}");
                    return discount;
                }

                throw new InvalidProgramException();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"There is no discount attached to customer type {customerType}", ex);
                return 0;
            }
        }
    }
}
