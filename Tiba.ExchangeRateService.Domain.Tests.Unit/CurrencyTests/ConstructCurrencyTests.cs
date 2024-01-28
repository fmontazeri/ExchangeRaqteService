using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class ConstructCurrencyTests
{
    private readonly TestCurrencyBuilder _builder;

    public ConstructCurrencyTests()
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
            .WithTimePeriod(TimePeriod.New(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.SOME_DAYS))).Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(null, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY)] //(null,3] [4,6]
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, null, DayConsts.THIRD_DAY)] //[4,6] (null,3] 
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, null)] //[1,3] [4,null)
    [InlineData(DayConsts.FORTH_DAY, null, DayConsts.FIRST_DAY, DayConsts.THIRD_DAY)] //[4,null) [1,3] 
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_The_Give_Open_Interval_List_of_Two_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
            .Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(null, DayConsts.THIRD_DAY, DayConsts.THIRD_DAY, DayConsts.SIXTH_DAY)] //(null,3] [3,6]
    [InlineData(DayConsts.FIFTH_DAY, null, DayConsts.THIRD_DAY, DayConsts.SIXTH_DAY)] //[1,null) [3,6]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, null, DayConsts.SIXTH_DAY)] //[1,3] (null,6] 
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.THIRD_DAY, null)] //[1,3] [3,null)
    [InlineData(null, null, null, null)]
    public void
        Constructor_Should_Construct_Currency_When_There_Is_Overlap_Between_The_Give_Open_Interval_List_of_Two_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var currency = _builder
                .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
                .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }

    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY)] //[1,3] [4,6]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY)] //[2,4] [5,7]
    [InlineData(DayConsts.THIRD_DAY, DayConsts.FIFTH_DAY, DayConsts.SIXTH_DAY, DayConsts.EIGHTH_DAY)] //[3,5] [6,8]
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, DayConsts.SEVENTH_DAY, DayConsts.NINTH_DAY)] //[4,6] [7,9]
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_The_Give_Close_Interval_List_of_Two_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
            .Build();

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.FORTH_DAY, DayConsts.FIRST_DAY, DayConsts.FORTH_DAY)] //[1,4] [1,4]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.FORTH_DAY, DayConsts.SECOND_DAY, DayConsts.FIFTH_DAY)] //[1,4] [2,5]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FIFTH_DAY, DayConsts.THIRD_DAY, DayConsts.SIXTH_DAY)] //[2,5] [3,6]
    [InlineData(DayConsts.THIRD_DAY, DayConsts.SIXTH_DAY, DayConsts.FORTH_DAY, DayConsts.SEVENTH_DAY)] //[3,6] [4,7]
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SEVENTH_DAY, DayConsts.FIFTH_DAY, DayConsts.EIGHTH_DAY)] //[4,7] [5,8]
    public void
        Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_The_Given_Close_Interval_TimePeriod_And_New_One(
            int? fromDate1,
            int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var currency = _builder
                .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
                .Build();

            currency.Add(TimePeriod.New(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }

    private (DateTime? from1, DateTime? to1, DateTime? from2, DateTime? to2) GetTimePeriod(int? fromDate1, int? toDate1,
        int? fromDate2, int? toDate2)
    {
        DateTime? from1 = fromDate1.HasValue ? DayConsts.TODAY.AddDays(fromDate1.Value) : null;
        DateTime? to1 = toDate1.HasValue ? DayConsts.TODAY.AddDays(toDate1.Value) : null;
        DateTime? from2 = fromDate2.HasValue ? DayConsts.TODAY.AddDays(fromDate2.Value) : null;
        DateTime? to2 = toDate2.HasValue ? DayConsts.TODAY.AddDays(toDate2.Value) : null;
        return (from1, to1, from2, to2);
    }
}