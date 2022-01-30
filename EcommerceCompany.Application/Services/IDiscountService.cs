namespace EcommerceCompany.Application.Services
{
    public interface IDiscountService
    {
        public double GetDiscountByCustomerType(string customerType);
    }
}
