using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

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

        actual.Name.Should().BeEquivalentTo(_builder.Money.Currency);
        actual.AssertCurrencyRates(options);
    }

    private Currency NewCurrency(ICurrencyRateOptions options)
    {
        return new Currency(options);
    }

    [Fact]
    public void Constructor_Should_Not_Initial_Currency_When_Currency_Is_Not_Defined()
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var options = _builder.WithMoney(null).BuildOptions();
            var actual = NewCurrency(options);
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }


    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.TENTH_DAY, DayConsts.SECOND_DAY,
        DayConsts.NINTH_DAY)]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.NINTH_DAY, DayConsts.FIRST_DAY,
        DayConsts.TENTH_DAY)]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.TENTH_DAY, DayConsts.FIRST_DAY,
        DayConsts.NINTH_DAY)]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.NINTH_DAY, DayConsts.NINTH_DAY,
        DayConsts.TENTH_DAY)]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.TENTH_DAY, DayConsts.FIRST_DAY,
        DayConsts.TENTH_DAY)]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.FIRST_DAY, DayConsts.FIRST_DAY,
        DayConsts.FIRST_DAY)]
    public void Constructor_Should_Not_Create_Currency_When_The_New_Time_Period_Overlaps_With_Others
        (int fromDate1, int toDate1, int fromDate2, int toDate2)
    {
        var options = _builder
            .WithTimePeriod(TimePeriod.New(DayConsts.TODAY.AddDays(fromDate1), DayConsts.TODAY.AddDays(toDate1)))
            .BuildOptions();
        var actual = NewCurrency(options);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            actual.Add(TimePeriod.New(DayConsts.TODAY.AddDays(fromDate2), DayConsts.TODAY.AddDays(toDate2)),
                CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }


    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.SECOND_DAY, DayConsts.NINTH_DAY,
        DayConsts.TENTH_DAY)]
    [InlineData(DayConsts.NINTH_DAY, DayConsts.TENTH_DAY, DayConsts.FIRST_DAY,
        DayConsts.SECOND_DAY)]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.FIRST_DAY, DayConsts.SECOND_DAY,
        DayConsts.SECOND_DAY)]
    public void Constructor_Should_Create_Currency_When_The_New_Time_Period_Doesnt_Has_Overlap_With_Others(
            int fromDate1, int toDate1, int fromDate2, int toDate2)
    {
        var currencyRate1 = _builder
            .WithTimePeriod(TimePeriod.New(DateTime.Today.AddDays(fromDate1), DateTime.Today.AddDays(toDate1)))
            .BuildOptions();
        var actual = NewCurrency(currencyRate1);

        actual.Add(TimePeriod.New(DateTime.Today.AddDays(fromDate2), DateTime.Today.AddDays(toDate2)), CurrencyConsts.SOME_PRICE);

        var expectedCurrencyRate2 = _builder
            .WithTimePeriod(TimePeriod.New(DateTime.Today.AddDays(fromDate2), DateTime.Today.AddDays(toDate2)))
            .BuildOptions();
        actual.AssertCurrencyRates(currencyRate1, expectedCurrencyRate2); //TODO : assert has error with equivalent
    }
}