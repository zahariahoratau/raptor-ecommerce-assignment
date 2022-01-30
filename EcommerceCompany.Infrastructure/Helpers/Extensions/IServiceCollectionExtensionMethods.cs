using EcommerceCompany.Application.Repositories;
using EcommerceCompany.Application.Services;
using EcommerceCompany.Infrastructure.Repositories;
using EcommerceCompany.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceCompany.Infrastructure.Helpers.Extensions
{
    public static class IServiceCollectionExtensionMethods
    {
        public static void AddInjectedDependencies(this IServiceCollection @this)
        {
            AddInjectedServices(@this);
            AddInjectedRepositories(@this);
        }

        private static void AddInjectedServices(IServiceCollection @this)
        {
            @this.AddScoped<IOrderService, OrderService>();
            @this.AddScoped<IDateTimeService, DateTimeService>();
            @this.AddScoped<ICustomerStatusServiceFactory, CustomerStatusServiceFactory>();
            @this.AddScoped<IDiscountService, DiscountService>();
        }

        private static void AddInjectedRepositories(IServiceCollection @this)
        {
            @this.AddScoped<IOrderRepository, OrderRepository>();
            @this.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
