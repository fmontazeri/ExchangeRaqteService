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
        var options = _builder.Build();

        var actual = NewCurrency(options);

        actual.Name.Should().BeEquivalentTo(_builder.Currency);
        actual.LastCurrencyRate.Should().BeEquivalentTo(options);
        actual.CurrencyRates.Should().AllSatisfy(ex => ex.Should().BeEquivalentTo(options));
    }

    private Currency NewCurrency(ICurrencyRateOptions options)
    {
        return new Currency(options);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_Not_Initial_Currency_When_CurrencyName_Is_Not_Defined(string currencyName)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var options = _builder.WithCurrency(currencyName).Build();
            var actual = NewCurrency(options);
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }

    [Fact]
    public void Constructor_Should_Not_Create_Currency_When_FromDate_Or_ToDate_Has_Not_Value()
    {
        var defaultDate = new DateTime();
        var actual = Assert.Throws<TheTimePeriodIsEmptyOrDefaultException>(() =>
        {
            var options = _builder.WithFromDate(defaultDate).WithToDate(defaultDate).Build();
            var actual = NewCurrency(options);
        });

        actual.Message.Should().BeEquivalentTo(TheTimePeriodIsEmptyOrDefaultException.ErrorMessage);
    }

    [Fact]
    public void Constructor_Should_Not_Create_Currency_When_The_New_Time_Period_Overlaps_With_The_Last_Time_Period()
    {
        var today = DateTime.Today;
        var options = _builder.WithFromDate(today).WithToDate(today.AddDays(CurrencyRateConsts.ONE_DAY)).Build();
        var actual = NewCurrency(options);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            actual.Add(today, today.AddDays(CurrencyRateConsts.SOME_DAYS),
                CurrencyRateConsts.SOME_PRICE);
        });

        var expectedMessage = string.Format(OverlapTimePeriodException.ErrorMessage, options.ToDate);
        exception.Message.Should().Be(expectedMessage);
    }
}