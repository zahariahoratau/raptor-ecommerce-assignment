using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;

namespace EcommerceCompany.Infrastructure.Services
{
    public class SilverCustomerStatusService : ICustomerStatusService
    {
        private readonly IDateTimeService _dateTimeService;

        public SilverCustomerStatusService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public bool CanAdvanceInStatus(Customer customer, int numberOfOrdersFromTheLast30Days, double valueOfOrdersFromTheLast30Days)
        {
            if (customer.StatusLastChangedDateUtc < _dateTimeService.DateTimeNowMinus7days &&
                customer.CustomerStatus == CustomerType.Silver &&
                valueOfOrdersFromTheLast30Days >= 600)
            {
                return true;
            }

            return false;
        }

        public string NextStatus { get; } = CustomerType.Gold;
    }
}
