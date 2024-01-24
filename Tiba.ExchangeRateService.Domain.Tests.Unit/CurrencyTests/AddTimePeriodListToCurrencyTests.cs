using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddTimePeriodListToCurrencyTests
{
    private readonly TestCurrencyBuilder _builder;

    public AddTimePeriodListToCurrencyTests()
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
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.Fifth_DAY, DayConsts.TENTH_DAY)] //(null,4] [5,10]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, DayConsts.Fifth_DAY, null)] //[1,4] [5,null)
    public void Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_The_Give_List_of_Two_TimePeriods(
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
    [InlineData(null, DayConsts.SECOND_DAY, DayConsts.FIRST_DAY, DayConsts.Forth_DAY)] //(null,2] [1,4]
    [InlineData(null, DayConsts.Forth_DAY, DayConsts.FIRST_DAY, DayConsts.SECOND_DAY)] //(null,4] [1,2]
    [InlineData(null, null, DayConsts.FIRST_DAY, DayConsts.Forth_DAY)] //(null,null) [1,4]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, null, null)] //[1,4] (null,null)
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, DayConsts.FIRST_DAY, DayConsts.SECOND_DAY)] //[1,4] [1,2]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, DayConsts.Forth_DAY, DayConsts.TENTH_DAY)] //[1,4] [4,10]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, null, DayConsts.TENTH_DAY)] //[1,4] (null,10]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.Forth_DAY, DayConsts.SECOND_DAY, null)] //[1,4] [2,null)
    [InlineData(null, null, null, null)] //(null,null) (null,null)
    public void Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_The_Given_TimePeriod_And_New_One(
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

    private (DateTime? from1, DateTime? to1, DateTime? from2, DateTime? to2, DateTime? from3, DateTime? to3)
        GetTimePeriod(int? fromDate1, int? toDate1,
            int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        DateTime? from1 = fromDate1.HasValue ? DayConsts.TODAY.AddDays(fromDate1.Value) : null;
        DateTime? to1 = toDate1.HasValue ? DayConsts.TODAY.AddDays(toDate1.Value) : null;
        DateTime? from2 = fromDate2.HasValue ? DayConsts.TODAY.AddDays(fromDate2.Value) : null;
        DateTime? to2 = toDate2.HasValue ? DayConsts.TODAY.AddDays(toDate2.Value) : null;

        // var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        DateTime? from3 = fromDate3.HasValue ? DayConsts.TODAY.AddDays(fromDate3.Value) : null;
        DateTime? to3 = toDate3.HasValue ? DayConsts.TODAY.AddDays(toDate3.Value) : null;
        // return (timePeriod.from1, timePeriod.to1, timePeriod.from2, timePeriod.to2, from3, to3);
        return (from1, to1, from2, to2, from3, to3);
    }


    [Fact]
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_New_TimePeriod_And_Two_Others()
    {
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.FIRST_DAY)))
            .WithTimePeriod(TimePeriod.New(DayConsts.TODAY.AddDays(DayConsts.SECOND_DAY),
                DayConsts.TODAY.AddDays(DayConsts.Third_DAY)))
            .Build();

        currency.Add(TimePeriod.New(DayConsts.TODAY.AddDays(DayConsts.Forth_DAY),
            DayConsts.TODAY.AddDays(DayConsts.Fifth_DAY)), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.SECOND_DAY, DayConsts.Third_DAY, DayConsts.Fifth_DAY,
        DayConsts.FIRST_DAY, null)]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.SECOND_DAY, DayConsts.Third_DAY, null, DayConsts.FIRST_DAY,
        DayConsts.Forth_DAY)]
    [InlineData(null, DayConsts.TENTH_DAY, DayConsts.SECOND_DAY, DayConsts.Third_DAY, DayConsts.FIRST_DAY,
        DayConsts.Fifth_DAY)]
    [InlineData(null, null, null, null, null, null)]
    public void
        Constructor_Should_Not_Construct_Currency_When_There_Is_Overlap_Between_New_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var currency = _builder
                .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
                .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
                .Build();

            currency.Add(TimePeriod.New(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);
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
            var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

            var currency = _builder
                .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
                .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }
}