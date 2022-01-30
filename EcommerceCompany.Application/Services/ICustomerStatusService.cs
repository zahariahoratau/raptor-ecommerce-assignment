using EcommerceCompany.Domain.Models;

namespace EcommerceCompany.Application.Services
{
    public interface ICustomerStatusService
    {
        public bool CanAdvanceInStatus(Customer customer, int numberOfOrdersFromTheLast30Days, double valueOfOrdersFromTheLast30Days);

        public string NextStatus { get; }
    }
}
