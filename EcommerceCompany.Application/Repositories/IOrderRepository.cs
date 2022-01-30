using EcommerceCompany.Domain.Models;

namespace EcommerceCompany.Application.Repositories
{
    public interface IOrderRepository
    {
        public bool Create(Order order);

        public Order? GetOrderById(int id);

        public Order[] GetAll();

        public Order[] GetAllByCustomerId(int customerId);

        public Order[] GetAllFromLast30DaysByCustomerId(int customerId);

        public bool ExistsById(int id);
    }
}
