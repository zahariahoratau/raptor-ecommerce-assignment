namespace EcommerceCompany.Application.Services
{
    public interface ICustomerStatusServiceFactory
    {
        public ICustomerStatusService? GetServiceByCustomerStatus(string customerStatus);
    }
}
