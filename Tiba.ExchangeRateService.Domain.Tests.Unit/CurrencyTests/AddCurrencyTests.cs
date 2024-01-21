using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddCurrencyTests
{
    private readonly TestCurrencyRateBuilder _builder;

    public AddCurrencyTests()
    {
        _builder = new TestCurrencyRateBuilder();
    }

    [Fact]
    public void Constructor_Should_Be_Initialize_Currency_Properly()
    {
        var actual = new Currency(_builder.Currency, _builder.FromDate, _builder.ToDate, _builder.Price);

        actual.Name.Should().BeEquivalentTo(_builder.Currency);
        var expectedExchangeRate = _builder.Build();
        actual.LastCurrencyRate.Should().BeEquivalentTo(expectedExchangeRate);
        actual.CurrencyRates.Should().AllSatisfy(ex => ex.Should().BeEquivalentTo(expectedExchangeRate));
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_Not_Initial_Currency_When_CurrencyName_Is_Not_Defined(string currencyName)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var actual = new Currency(currencyName, DateTime.Today, DateTime.Today.AddDays(1), 10000);
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }

    [Fact]
    public void Constructor_Should_Not_Create_Currency_When_FromDate_Or_ToDate_Has_Not_Value()
    {
        var defaultDate = new DateTime();
        var actual = Assert.Throws<TheTimePeriodIsEmptyOrDefaultException>(() =>
        {
            var actual = new Currency("USD", defaultDate, defaultDate, 1200);
        });

        actual.Message.Should().BeEquivalentTo(TheTimePeriodIsEmptyOrDefaultException.ErrorMessage);
    }

    [Fact]
    public void Constructor_Should_Not_Create_Currency_When_The_New_Time_Period_Overlaps_With_The_Last_Time_Period()
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