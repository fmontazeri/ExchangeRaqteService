using FluentAssertions;
using Tiba.ExchangeRateService.Domain.ExchangeRates;
using Tiba.ExchangeRateService.Domain.ExchangeRates.Exceptions;


namespace Tiba.ExchangeRateService.Domain.Tests.Unit.ExchangeRatesTests;

public class AddCurrencyTests
{
    [Fact]
    public void Constructor_Should_Be_Initialize_Properly()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var actual = new Currency("USD", today, tomorrow, 1200);

        actual.Name.Should().BeEquivalentTo("USD");
        var expectedExchangeRate = 
            new ExchangeRateBuilder()
                .WithFromDate(today).WithToDate(tomorrow).WithPrice(1200)
                .Build();
        actual.ExchangeRates.First().Should().BeEquivalentTo(expectedExchangeRate);
    }

    [Fact]
    public void Constructor_Should_Not_Be_Constructed_When_The_New_Time_Period_Overlaps_With_The_Last_Time_Period()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var afterTomorrow = tomorrow.AddDays(2);
        var actual = new Currency("USD", today, tomorrow, 46000);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            actual.Add(tomorrow, afterTomorrow, 54000);
        });

        var expectedMessage = string.Format(OverlapTimePeriodException.ErrorMessage, tomorrow);
        exception.Message.Should().Be(expectedMessage);
    }
}