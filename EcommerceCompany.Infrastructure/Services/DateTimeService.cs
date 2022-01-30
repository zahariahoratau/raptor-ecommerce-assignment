using EcommerceCompany.Application.Services;

namespace EcommerceCompany.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime DateTimeNowUtc => DateTime.Now;
        public DateTime DateTimeNowMinus30Days => DateTimeNowUtc.AddDays(-30);
        public DateTime DateTimeNowMinus7days => DateTimeNowUtc.AddDays(-7);
    }
}
