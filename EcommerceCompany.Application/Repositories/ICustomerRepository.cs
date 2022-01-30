using EcommerceCompany.Domain.Models;

namespace EcommerceCompany.Application.Repositories
{
    public interface ICustomerRepository
    {
        public Customer? GetCustomerById(int id);

        public Customer[]? GetAll();

        public bool Update(Customer customer);

        public bool ExistsById(int id);
    }
}
