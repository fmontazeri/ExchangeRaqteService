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
        var options = _builder.BuildOptions();

        var actual = NewCurrency(options);

        actual.Name.Should().BeEquivalentTo(_builder.Currency);
        actual.AssertCurrencyRates(options);
    }

    private Currency NewCurrency(ICurrencyRateOptions options)
    {
        return new Currency(options);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_Not_Initial_Currency_When_Currency_Is_Not_Defined(string currencyName)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var options = _builder.WithCurrency(currencyName).Build();
            var actual = NewCurrency(options);
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }



    [Theory]
    [InlineData(TimePeriod.FIRST_DAY, TimePeriod.TENTH_DAY, TimePeriod.SECOND_DAY, TimePeriod.NINTH_DAY)]
    [InlineData(TimePeriod.SECOND_DAY, TimePeriod.NINTH_DAY, TimePeriod.FIRST_DAY, TimePeriod.TENTH_DAY)]
    [InlineData(TimePeriod.SECOND_DAY, TimePeriod.TENTH_DAY, TimePeriod.FIRST_DAY, TimePeriod.NINTH_DAY)]
    [InlineData(TimePeriod.FIRST_DAY, TimePeriod.NINTH_DAY, TimePeriod.NINTH_DAY, TimePeriod.TENTH_DAY)]
    [InlineData(TimePeriod.FIRST_DAY, TimePeriod.TENTH_DAY, TimePeriod.FIRST_DAY, TimePeriod.TENTH_DAY)]
    [InlineData(TimePeriod.FIRST_DAY, TimePeriod.FIRST_DAY, TimePeriod.FIRST_DAY, TimePeriod.FIRST_DAY)]
    public void Constructor_Should_Not_Create_Currency_When_The_New_Time_Period_Overlaps_With_Others
        (int fromDate1, int toDate1, int fromDate2, int toDate2)
    {
        var options = _builder
            .WithFromDate(TimePeriod.TODAY.AddDays(fromDate1))
            .WithToDate(TimePeriod.TODAY.AddDays(toDate1)).BuildOptions();
        var actual = NewCurrency(options);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var newCurrencyRateOptions = _builder
                .WithFromDate(TimePeriod.TODAY.AddDays(fromDate2))
                .WithToDate(TimePeriod.TODAY.AddDays(toDate2))
                .BuildOptions();
            actual.Add(newCurrencyRateOptions);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }


    [Theory]
    [InlineData(TimePeriod.FIRST_DAY, TimePeriod.SECOND_DAY, TimePeriod.NINTH_DAY, TimePeriod.TENTH_DAY)]
    [InlineData(TimePeriod.NINTH_DAY, TimePeriod.TENTH_DAY, TimePeriod.FIRST_DAY, TimePeriod.SECOND_DAY)]
    [InlineData(TimePeriod.FIRST_DAY, TimePeriod.FIRST_DAY, TimePeriod.SECOND_DAY, TimePeriod.SECOND_DAY)]
    public void Constructor_Should_Create_Currency_When_The_New_Time_Period_Doesnt_Has_Overlap_With_The_Last_Time_Period(
            int fromDate1, int toDate1, int fromDate2, int toDate2)
    {
        var options = _builder
            .WithFromDate(TimePeriod.TODAY.AddDays(fromDate1))
            .WithToDate(TimePeriod.TODAY.AddDays(toDate1)).Build();
        var actual = NewCurrency(options);
        var newRateOptions = _builder
            .WithFromDate(TimePeriod.TODAY.AddDays(fromDate2))
            .WithToDate(TimePeriod.TODAY.AddDays(toDate2))
            .BuildOptions();
        
        actual.Add(newRateOptions);

        actual.AssertCurrencyRates(options, newRateOptions);
    }
}