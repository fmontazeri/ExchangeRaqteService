using FluentAssertions;
using Tiba.ExchangeRateService.Domain.ExchangeRates;
using Tiba.ExchangeRateService.Domain.ExchangeRates.Exceptions;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.ExchangeRatesTests;

public class AddExchangeRateTests
{
    [Theory]
    [InlineData(1, 200)]
    [InlineData(3, 450)]
    public void Constructor_Should_Initial_ExchangeRate_Correctly(int theDayAfterTomorrow, decimal price)
    {
        var today = DateTime.Today;
        var nextDay = today.AddDays(theDayAfterTomorrow);
        var actual = new ExchangeRate(today, nextDay, price);

        actual.FromDateDate.Should().Be(today);
        actual.ToDateDate.Should().Be(nextDay);
        actual.FromDateDate.Should().BeBefore(actual.ToDateDate);
        actual.Price.Should().BeGreaterThan(0);
        actual.Price.Should().Be(price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(-120000)]
    public void ExchangeRate_Should_Not_Be_Constructed_When_Price_Is_Not_Valid(decimal price)
    {
        var exception = Assert.Throws<PriceIsNotValidException>(() =>
        {
            var actual = new ExchangeRate(DateTime.Now, DateTime.Now, price);
        });

        exception.Message.Should().BeEquivalentTo(PriceIsNotValidException.ErrorMessage);
    }

    [Fact]
    public void ExchangeRate_Should_Not_Be_Constructed_When_FromDate_Is_After_The_ToDate()
    {
        var today = DateTime.Today;
        var previousDay = today.AddDays(-1);

        var exception = Assert.Throws<FromDateIsNotValidException>(() =>
        {
            var actual = new ExchangeRate(previousDay, today, 120000);
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }
}