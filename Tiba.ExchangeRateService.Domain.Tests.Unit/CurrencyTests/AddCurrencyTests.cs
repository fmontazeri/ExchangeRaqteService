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

    [Theory]
    [InlineData(1, 10, 2, 5)]
    [InlineData(3, 7, 1, 10)]
    [InlineData(3, 10, 1, 5)]
    [InlineData(1, 8, 6, 10)]
    public void Constructor_Should_Not_Create_Currency_When_The_New_Time_Period_Overlaps_With_The_Last_Time_Period
        (int fromDate1, int toDate1, int fromDate2, int toDate2)
    {
        var today = DateTime.Today;
        var options = _builder.WithFromDate(today.AddDays(fromDate1)).WithToDate(today.AddDays(toDate1)).Build();
        var actual = NewCurrency(options);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            actual.Add(today.AddDays(fromDate2), today.AddDays(toDate2),
                CurrencyRateConsts.SOME_PRICE);
        });

        var expectedMessage = string.Format(OverlapTimePeriodException.ErrorMessage, options.ToDate);
        exception.Message.Should().Be(expectedMessage);
    }


    [Theory]
    [InlineData(1, 10, 11, 15)]
    [InlineData(3, 7, 8, 20)]
    public void Constructor_Should_Create_Currency_When_The_New_Time_Period_Doesnt_Has_Overlap_With_The_Last_Time_Period(int fromDate1, int toDate1, int fromDate2, int toDate2)
    {
        var today = DateTime.Today;
        var options = _builder.WithFromDate(today.AddDays(fromDate1)).WithToDate(today.AddDays(toDate1)).Build();
        var actual = NewCurrency(options);

        actual.Add(today.AddDays(fromDate2), today.AddDays(toDate2),
            CurrencyRateConsts.SOME_PRICE);

        //Need to more assertion
        actual.CurrencyRates.Should().HaveCount(2);
    }
}