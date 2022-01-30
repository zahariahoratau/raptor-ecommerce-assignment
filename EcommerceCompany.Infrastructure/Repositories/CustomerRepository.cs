using EcommerceCompany.Application.Repositories;
using EcommerceCompany.Domain.Models;
using EcommerceCompany.Infrastructure.Helpers.Extensions;
using EcommerceCompany.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace EcommerceCompany.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private static DbCustomer[] Customers { get; set; } = new DbCustomer[]
        {
            new DbCustomer
            {
                Id = 1,
                Name = "Someone",
                CustomerStatus = CustomerType.Silver,
                MemberSinceDateUtc = DateTime.UtcNow,
                StatusLastChangedDateUtc = DateTime.UtcNow.AddDays(-20),
            }
        };

        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            ILogger<CustomerRepository> logger)
        {
            _logger = logger;
        }

        public Customer? GetCustomerById(int id)
        {
            try
            {
                var dbCustomer = Customers.FirstOrDefault((customer) => customer.Id == id);

                dbCustomer.ThrowIfNull();

                return MapDbCustomerToCustomer(dbCustomer!);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the customer", ex);
                return default;
            }
        }

        public Customer[]? GetAll()
        {
            try
            {
                return Customers
                    .Select((customer) => MapDbCustomerToCustomer(customer))
                    .ToArray();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the customers", ex);
                return default;
            }
        }

        public bool Update(Customer customer)
        {
            try
            {
                int customerToUpdateIndex = -1;

                for (int i=0; i < Customers.Length; i++)
                    if (Customers[i].Id == customer.Id)
                        customerToUpdateIndex = i;

                if (customerToUpdateIndex < 0)
                    throw new ArgumentException("No customer with this id");

                Customers[customerToUpdateIndex] = MapCustomerToDbCustomer(customer);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating the customer", ex);
                return false;
            }
        }

        public bool ExistsById(int id)
        {
            try
            {
                if (Customers.Any((customer) => customer.Id == id))
                    return true;

                return false;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("An error occured when fetching the result", ex);
                return false;
            }
        }

        private DbCustomer MapCustomerToDbCustomer(Customer customer)
        {
            return new DbCustomer
            {
                Id = customer.Id,
                Name = customer.Name ?? string.Empty,
                CustomerStatus = customer.CustomerStatus,
                MemberSinceDateUtc = customer.MemberSinceDateUtc,
                StatusLastChangedDateUtc = customer.StatusLastChangedDateUtc
            };
        }

        private Customer MapDbCustomerToCustomer(DbCustomer dbCustomer)
        {
            return new Customer
            {
                Id = dbCustomer.Id,
                Name = dbCustomer.Name,
                CustomerStatus = dbCustomer.CustomerStatus,
                MemberSinceDateUtc = dbCustomer.MemberSinceDateUtc,
                StatusLastChangedDateUtc = dbCustomer.StatusLastChangedDateUtc
            };
        }
    }
}
