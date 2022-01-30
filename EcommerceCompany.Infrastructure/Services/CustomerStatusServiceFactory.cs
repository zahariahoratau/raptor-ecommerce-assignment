using EcommerceCompany.Application.Services;
using EcommerceCompany.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceCompany.Infrastructure.Services
{
    public class CustomerStatusServiceFactory : ICustomerStatusServiceFactory
    {
        private readonly IDictionary<string, ICustomerStatusService> _customerStatusServices = new Dictionary<string, ICustomerStatusService>();

        private readonly ILogger<CustomerStatusServiceFactory> _logger;

        public CustomerStatusServiceFactory(
            ILogger<CustomerStatusServiceFactory> logger,
            IDateTimeService dateTimeService)
        {
            _logger = logger;

            _customerStatusServices.Add(CustomerType.Regular, new RegularCustomerStatusService(dateTimeService));
            _customerStatusServices.Add(CustomerType.Silver, new SilverCustomerStatusService(dateTimeService));
        }

        public ICustomerStatusService? GetServiceByCustomerStatus(string customerStatus)
        {
            try
            {
                var result = _customerStatusServices.TryGetValue(customerStatus, out ICustomerStatusService? customerStatusService);

                if (result)
                    return customerStatusService;

                throw new InvalidProgramException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Factory could not identify service with the customer status {customerStatus}", ex);
                return null;
            }
        }
    }
}
