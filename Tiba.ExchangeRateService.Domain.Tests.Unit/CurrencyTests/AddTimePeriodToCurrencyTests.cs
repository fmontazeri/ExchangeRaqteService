using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddTimePeriodToCurrencyTests : BaseCurrencyTests
{
    private readonly TestCurrencyBuilder _builder;
    public AddTimePeriodToCurrencyTests()
    {
        _builder = new TestCurrencyBuilder();
    }

    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY)] //[1,3] [4,6]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY)] //[2,4] [5,7]
    [InlineData(DayConsts.THIRD_DAY, DayConsts.FIFTH_DAY, DayConsts.SIXTH_DAY, DayConsts.EIGHTH_DAY)] //[3,5] [6,8]
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, DayConsts.SEVENTH_DAY, DayConsts.NINTH_DAY)] //[4,6] [7,9]
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_New_Close_Interval_TimePeriod_And_Another_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .Build();

        currency.Add(TimePeriod.New(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(DayConsts.THIRD_DAY, DayConsts.EIGHTH_DAY, DayConsts.THIRD_DAY, DayConsts.EIGHTH_DAY)] //[3,8] [3,8]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.SEVENTH_DAY, DayConsts.FORTH_DAY, DayConsts.NINTH_DAY)] //[2,7] [4,9]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.SIXTH_DAY, DayConsts.FIFTH_DAY, DayConsts.TENTH_DAY)] //[1,6] [5,10]
    public void
        Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_The_Given_Close_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(TimePeriod.New(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(null, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY)] //(null,3] [4,6]
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, null, DayConsts.THIRD_DAY)] //[4,6] (null,3] 
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, null)] //[1,3] [4,null)
    [InlineData(DayConsts.FORTH_DAY, null, DayConsts.FIRST_DAY, DayConsts.THIRD_DAY)] //[4,null) [1,3] 
    public void
        Constructor_Should_Not_Construct_When_There_Is_Not_Overlap_Between_The_Given_Open_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .Build();

        currency.Add(TimePeriod.New(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);

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
        Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_The_Given_Open_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .Build();
        
        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(TimePeriod.New(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, DayConsts.SEVENTH_DAY, DayConsts.NINTH_DAY)] //[1,3] [4,6] [7,9]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY, DayConsts.EIGHTH_DAY, DayConsts.TENTH_DAY)] //[2,4] [5,7] [8,10] 
    [InlineData(DayConsts.THIRD_DAY, DayConsts.FIFTH_DAY, DayConsts.SIXTH_DAY, DayConsts.EIGHTH_DAY, DayConsts.FIRST_DAY, DayConsts.SECOND_DAY)] //[3,5] [6,8] [1,2]
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, DayConsts.SEVENTH_DAY, DayConsts.NINTH_DAY, DayConsts.FIRST_DAY, DayConsts.THIRD_DAY)] //[4,6] [7,9] [1,3] 
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_New_Close_Interval_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
            .Build();

        currency.Add(TimePeriod.New(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.FIRST_DAY, DayConsts.TENTH_DAY)] //[1,3] [4,6] [1,10]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.SECOND_DAY, DayConsts.FIFTH_DAY)] //[2,4] [5,7] [2,5]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.THIRD_DAY, DayConsts.EIGHTH_DAY)] //[2,4] [5,7] [3,8]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.FORTH_DAY, DayConsts.SEVENTH_DAY)] //[2,4] [5,7] [4,7]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY)] //[2,4] [5,7] [4,5]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.FIRST_DAY, DayConsts.SECOND_DAY)] //[2,4] [5,7] [1,2]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.FIRST_DAY, DayConsts.FIFTH_DAY)] //[2,4] [5,7] [1,5]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.FORTH_DAY, DayConsts.TENTH_DAY)] //[2,4] [5,7] [4,10]
    [InlineData(DayConsts.SECOND_DAY, DayConsts.FORTH_DAY, DayConsts.FIFTH_DAY, DayConsts.SEVENTH_DAY,
        DayConsts.SEVENTH_DAY, DayConsts.TENTH_DAY)] //[2,4] [5,7] [7,10]
    public void
        Constructor_Should_Not_Construct_Currency_When_There_Is_Overlap_Between_New_Close_Interval_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(TimePeriod.New(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(2);
    }


    [Theory]
    [InlineData(null, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, DayConsts.SEVENTH_DAY,
        null)] //(null,3] [4,6] [7,null)
    [InlineData(DayConsts.SECOND_DAY, DayConsts.THIRD_DAY, null, DayConsts.FIRST_DAY, DayConsts.FORTH_DAY,
        DayConsts.SEVENTH_DAY)] //[2,3] (null,1] [4,7)
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, null, DayConsts.THIRD_DAY, DayConsts.SEVENTH_DAY,
        null)] //[4,6] (null,3] [7,null)
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.SIXTH_DAY, null, DayConsts.FORTH_DAY,
        DayConsts.FIFTH_DAY)] //[1,3] [6,null) [4,5]
    public void
        Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_New_Open_Interval_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
            .Build();

        currency.Add(TimePeriod.New(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(null, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, DayConsts.SIXTH_DAY,
        null)] //(null,3] [4,6] [6,null)
    [InlineData(DayConsts.SECOND_DAY, DayConsts.THIRD_DAY, null, DayConsts.FIRST_DAY, DayConsts.SECOND_DAY,
        null)] //[2,3] (null,1] [2,null)
    [InlineData(DayConsts.FORTH_DAY, DayConsts.SIXTH_DAY, null, DayConsts.THIRD_DAY, DayConsts.FORTH_DAY,
        null)] //[4,6] (null,3] [4,null)
    [InlineData(DayConsts.FIRST_DAY, DayConsts.THIRD_DAY, DayConsts.SIXTH_DAY, null, DayConsts.FORTH_DAY,
        DayConsts.EIGHTH_DAY)] //[1,3] [6,null) [4,8]
    public void
        Constructor_Should_Construct_Currency_When_There_Is_Overlap_Between_New_Open_Interval_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
            .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(TimePeriod.New(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);
        });
        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
       currency.CurrencyRates.Should().HaveCount(2);
    }

    // [Theory]
    // [InlineData(null, DayConsts.FORTH_DAY, DayConsts.FIRST_DAY, DayConsts.FORTH_DAY)] //(null,4] [1,4]
    // [InlineData(null, DayConsts.FORTH_DAY, DayConsts.FIRST_DAY, DayConsts.THIRD_DAY)] //(null,4] [1,3]
    // [InlineData(null, DayConsts.FORTH_DAY, DayConsts.FIRST_DAY, DayConsts.FIFTH_DAY)] //(null,4] [1,5]
    // [InlineData(null, DayConsts.FORTH_DAY, null, DayConsts.THIRD_DAY)] //(null,4] (null,3]
    // [InlineData(null, DayConsts.FORTH_DAY, DayConsts.THIRD_DAY, null)] //(null,4] [3,null)
    // [InlineData(null, DayConsts.FORTH_DAY, DayConsts.FORTH_DAY, null)] //(null,4] [4,null)
    // [InlineData(null, null, null, null)] //(null,null) (null,null)
    // [InlineData(DayConsts.FIRST_DAY, null, DayConsts.FIRST_DAY, DayConsts.FORTH_DAY)] // [1,null) [1,4]
    // [InlineData(DayConsts.FIRST_DAY, DayConsts.FORTH_DAY, null, DayConsts.THIRD_DAY)] // [1,4] (null,3]
    // [InlineData(DayConsts.FIRST_DAY, DayConsts.FORTH_DAY, null, DayConsts.FIFTH_DAY)] // [1,4] (null,5]
    // [InlineData(DayConsts.FIRST_DAY, null, null, DayConsts.FIRST_DAY)] // [1,null) (null,1]
    // [InlineData(DayConsts.FIRST_DAY, null, null, DayConsts.FORTH_DAY)] // [1,null) (null,4]
    // [InlineData(DayConsts.FIRST_DAY, DayConsts.FORTH_DAY, null, null)] // [1,4] (null,null)
    // [InlineData(null, null, 1, DayConsts.FORTH_DAY)] // (null,null) [1,4] 
    // public void
    //     Constructor_Should_Not_Construct_When_There_Is_Overlap_Between_Given_Open_Interval_Time_Periods_And_New_One(
    //         int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    // {
    //     var exception = Assert.Throws<OverlapTimePeriodException>(() =>
    //     {
    //         var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
    //
    //         var currency = _builder
    //             .WithTimePeriod(TimePeriod.New(timePeriod.from1, timePeriod.to1))
    //             .WithTimePeriod(TimePeriod.New(timePeriod.from2, timePeriod.to2))
    //             .Build();
    //     });
    //
    //     exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    // }
}