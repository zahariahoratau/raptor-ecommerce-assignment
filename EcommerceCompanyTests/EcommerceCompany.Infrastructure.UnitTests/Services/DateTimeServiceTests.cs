using EcommerceCompany.Infrastructure.Services;
using NSubstitute;
using System;
using Xunit;

namespace EcommerceCompanyTests.EcommerceCompany.Infrastructure.UnitTests.Services
{
    public class DateTimeServiceTests
    {
        private readonly DateTimeService _sut;

        public DateTimeServiceTests()
        {
            _sut = new DateTimeService();
        }

        [Fact]
        public void DateTimeNowUtc_ShouldReturnToday()
        {
            Assert.Equal(DateTime.UtcNow.Year, _sut.DateTimeNowUtc.Year);
            Assert.Equal(DateTime.UtcNow.Month, _sut.DateTimeNowUtc.Month);
            Assert.Equal(DateTime.UtcNow.Day, _sut.DateTimeNowUtc.Day);
        }

        [Fact]
        public void DateTimeNowMinus7Days_ShouldReturnTodayMinus7Days()
        {
            Assert.Equal(DateTime.UtcNow.Year, _sut.DateTimeNowMinus7days.Year);
            Assert.Equal(DateTime.UtcNow.Month, _sut.DateTimeNowMinus7days.Month);
            Assert.Equal(DateTime.UtcNow.Day - 7, _sut.DateTimeNowMinus7days.Day);
        }

        [Fact]
        public void DateTimeNowMinus30Days_ShouldReturnTodayMinus7Days()
        {
            DateTime nowMinus30DaysUtc = DateTime.UtcNow.Subtract(new TimeSpan(30 * 24, 0, 0));

            Assert.Equal(nowMinus30DaysUtc.Year, _sut.DateTimeNowMinus30Days.Year);
            Assert.Equal(nowMinus30DaysUtc.Month, _sut.DateTimeNowMinus30Days.Month);
            Assert.Equal(nowMinus30DaysUtc.Day, _sut.DateTimeNowMinus30Days.Day);
        }
    }
}
