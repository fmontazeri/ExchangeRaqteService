using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddListToCurrencyTests
{
    private readonly TestCurrencyBuilder _builder;

    public AddListToCurrencyTests()
    {
        _builder = new TestCurrencyBuilder();
    }

    [Fact]
    public void Constructor_Should_Construct_Currency_With_No_TimePeriods_Successfully()
    {
        var currency = _builder.Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(0);
    }

    [Fact]
    public void Constructor_Should_Construct_Currency_With_One_TimePeriod_Successfully()
    {
        var currency = _builder
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.SOME_DAYS))).Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(1);
    }

    [Fact]
    public void Constructor_Should_Construct_Currency_With_Two_TimePeriods_Successfully()
    {
        var currency = _builder
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.FIRST_DAY)))
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY.AddDays(DayConsts.SECOND_DAY), DayConsts.TODAY.AddDays(DayConsts.Third_DAY)))
            .Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Fact]
    public void Constructor_Should_Not_Construct_Currency_With_Two_TimePeriods()
    {
        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var currency = _builder
                .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.SOME_DAYS)))
                .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.SOME_DAYS)))
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }


    [Fact]
    public void Constructor_Should_Construct_Currency_With_Three_TimePeriods_Successfully()
    {
        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var currency = _builder
                .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.FIRST_DAY)))
                .WithTimePeriod(new TimePeriod(DayConsts.TODAY.AddDays(DayConsts.SECOND_DAY),
                    DayConsts.TODAY.AddDays(DayConsts.Third_DAY)))
                .WithTimePeriod(new TimePeriod(DayConsts.TODAY.AddDays(DayConsts.Third_DAY),
                    DayConsts.TODAY.AddDays(DayConsts.Fifth_DAY)))
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }
}