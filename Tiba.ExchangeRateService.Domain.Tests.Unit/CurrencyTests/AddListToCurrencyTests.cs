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
    public void Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_The_Give_Two_TimePeriods()
    {
        var currency = _builder
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.FIRST_DAY)))
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY.AddDays(DayConsts.SECOND_DAY),
                DayConsts.TODAY.AddDays(DayConsts.Third_DAY)))
            .Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Fact]
    public void Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_The_Given_Two_TimePeriods()
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
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_The_Last_TimePeriod_With_The_Two_Others()
    {
        var currency = _builder
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.FIRST_DAY)))
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY.AddDays(DayConsts.SECOND_DAY),
                DayConsts.TODAY.AddDays(DayConsts.Third_DAY)))
            .WithTimePeriod(new TimePeriod(DayConsts.TODAY.AddDays(DayConsts.Forth_DAY),
                DayConsts.TODAY.AddDays(DayConsts.Fifth_DAY)))
            .Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(3);
    }

    [Fact]
    public void Constructor_Should_Not_Construct_Currency_When_There_Is_Overlap_Between_Last_PeriodTime_With_Two_Others()
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


    [Theory]
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.FIRST_DAY, DayConsts.Forth_DAY)] //(null,4] [1,4]
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.FIRST_DAY, DayConsts.Third_DAY)] //(null,4] [1,3]
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.FIRST_DAY, DayConsts.Fifth_DAY)] //(null,4] [1,5]
    [InlineData(null, DayConsts.Forth_DAY, null, DayConsts.Third_DAY)] //(null,4] (null,3]
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.Third_DAY, null)] //(null,4] [3,null)
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.Forth_DAY, null)] //(null,4] [4,null)
    [InlineData(null, null, null, null)] //(null,null) (null,null)
    [InlineData(DayConsts.FIRST_DAY, null, DayConsts.FIRST_DAY, DayConsts.Forth_DAY)] // [1,null) [1,4]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, null, DayConsts.Third_DAY)] // [1,4] (null,3]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, null, DayConsts.Fifth_DAY)] // [1,4] (null,5]
    [InlineData(DayConsts.FIRST_DAY, null, null, DayConsts.FIRST_DAY)] // [1,null) (null,1]
    [InlineData(DayConsts.FIRST_DAY, null, null, DayConsts.Forth_DAY)] // [1,null) (null,4]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, null, null)] // [1,4] (null,null)
    [InlineData(null, null, 1, DayConsts.Forth_DAY)] // (null,null) [1,4] 
    public void Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_Time_Periods(int? fromDate1,
        int? toDate1, int? fromDate2, int? toDate2)
    {
        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            DateTime? from1 = fromDate1.HasValue ? DayConsts.TODAY.AddDays(fromDate1.Value) : null;
            DateTime? to1 = toDate1.HasValue ? DayConsts.TODAY.AddDays(toDate1.Value) : null;
            DateTime? from2 = fromDate2.HasValue ? DayConsts.TODAY.AddDays(fromDate2.Value) : null;
            DateTime? to2 = toDate2.HasValue ? DayConsts.TODAY.AddDays(toDate2.Value) : null;

            var currency = _builder
                .WithTimePeriod(new TimePeriod(from1, to1))
                .WithTimePeriod(new TimePeriod(from2, to2))
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }
}