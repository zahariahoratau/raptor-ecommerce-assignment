using EcommerceCompany.Application.Models;

namespace EcommerceCompany.Application.Services
{
    public interface IOrderService
    {
        public void CalculatePriceAndCreateOrderFromOrderMessage(DtoOrder dtoOrder);
    }
}
