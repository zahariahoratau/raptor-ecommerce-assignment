namespace EcommerceCompany.Application.Services
{
    public interface IDateTimeService
    {
        public DateTime DateTimeNowUtc { get; }

        public DateTime DateTimeNowMinus30Days { get; }

        public DateTime DateTimeNowMinus7days { get; }
    }
}
