using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;

namespace EcommerceCompany.Infrastructure.Services
{
    public class RegularCustomerStatusService : ICustomerStatusService
    {
        private readonly IDateTimeService _dateTimeService;

        public RegularCustomerStatusService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public bool CanAdvanceInStatus(Customer customer, int numberOfOrdersFromTheLast30Days, double valueOfOrdersFromTheLast30Days)
        {
            if (customer.StatusLastChangedDateUtc < _dateTimeService.DateTimeNowMinus7days &&
                customer.CustomerStatus == CustomerType.Regular &&
                numberOfOrdersFromTheLast30Days >= 1 &&
                valueOfOrdersFromTheLast30Days >= 300)
            {
                return true;
            }

            return false;
        }

        public string NextStatus { get; } = CustomerType.Silver;
    }
}
