using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

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
        var actual = new ExchangeRateBuilder()
            .WithFromDate(today).WithToDate(nextDay).WithPrice(price)
            .Build();

        actual.FromDate.Should().Be(today);
        actual.ToDate.Should().Be(nextDay);
        actual.FromDate.Should().BeBefore(actual.ToDate);
        actual.Price.Should().BeGreaterThan(0);
        actual.Price.Should().Be(price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(-120000)]
    public void ExchangeRate_Should_Not_Be_Added_When_Price_Is_Not_Valid(decimal price)
    {
        var exception = Assert.Throws<PriceIsNotValidException>(() =>
        {
            var actual = new ExchangeRateBuilder()
                .WithFromDate(DateTime.Now).WithToDate(DateTime.Now).WithPrice(price)
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(PriceIsNotValidException.ErrorMessage);
    }


    [Fact]
    public void ExchangeRate_Should_Be_Added_When_The_FromDate_Is_Before_Than_The_ToDate()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var actual = new ExchangeRateBuilder()
            .WithFromDate(today).WithToDate(tomorrow).WithPrice(120000)
            .Build();

        actual.FromDate.Should().Be(today);
        actual.ToDate.Should().Be(tomorrow);
        actual.FromDate.Should().BeBefore(actual.ToDate);
    }

    [Fact]
    public void ExchangeRate_Should_Not_Be_Constructed_When_FromDate_Is_After_The_ToDate()
    {
        var today = DateTime.Today;
        var nextDay = today.AddDays(2);

        var exception = Assert.Throws<FromDateIsNotValidException>(() =>
        {
            var actual = new ExchangeRateBuilder()
                .WithFromDate(nextDay).WithToDate(today).WithPrice(120000)
                .Build();
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }

    [Fact]
    public void ExchangeRate_Should_Be_Update_Correctly()
    {
        //
    }
}