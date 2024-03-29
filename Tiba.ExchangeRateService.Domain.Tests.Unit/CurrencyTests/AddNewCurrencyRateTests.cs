using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddNewCurrencyRateTests : BaseCurrencyTests
{
    private readonly CurrencyTestBuilder _builder;

    public AddNewCurrencyRateTests()
    {
        _builder = new CurrencyTestBuilder();
    }

    [Theory]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FORTH_DAY, Days.SIXTH_DAY)] //[2,3] [4,6]
    [InlineData(Days.FORTH_DAY, Days.SIXTH_DAY, Days.FIRST_DAY, Days.THIRD_DAY)] //[4,6] [2,3]
    public void
        Add_Should_Add_A_New_Currency_When_There_Is_No_Overlap_Between_The_Given_Close_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .Build();

        currency.Add(new TimePeriodOptionsTest(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(Days.THIRD_DAY, Days.EIGHTH_DAY, Days.THIRD_DAY, Days.EIGHTH_DAY)] //[3,8] [3,8]
    [InlineData(Days.SECOND_DAY, Days.SEVENTH_DAY, Days.FORTH_DAY, Days.NINTH_DAY)] //[2,7] [4,9]
    [InlineData(Days.FIRST_DAY, Days.SIXTH_DAY, Days.FIFTH_DAY, Days.TENTH_DAY)] //[1,6] [5,10]
    public void
        Add_Should_Not_Add_A_New_Currency_When_There_Is_Overlap_Between_The_Given_Close_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(new TimePeriodOptionsTest(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(null, Days.THIRD_DAY, Days.FORTH_DAY, Days.SIXTH_DAY)] //(null,3] [4,6]
    [InlineData(Days.FORTH_DAY, Days.SIXTH_DAY, null, Days.THIRD_DAY)] //[4,6] (null,3] 
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FORTH_DAY, null)] //[1,3] [4,null)
    [InlineData(Days.FORTH_DAY, null, Days.FIRST_DAY, Days.THIRD_DAY)] //[4,null) [1,3] 
    public void
        Add_Should_Not_Add_New_Currency_When_There_Is_No_Overlap_Between_The_Given_Open_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .Build();

        currency.Add(new TimePeriodOptionsTest(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(2);
    }

    [Theory]
    // [InlineData(null, Days.FIFTH_DAY, Days.SECOND_DAY, Days.FORTH_DAY)] //(null,5] [2,4]
    // [InlineData(null, Days.FIFTH_DAY, Days.THIRD_DAY, Days.FIFTH_DAY)] //(null,5] [3,5]
    // [InlineData(null, Days.FIFTH_DAY, Days.FORTH_DAY, Days.SIXTH_DAY)] //(null,5] [4,6]
    // [InlineData(null, Days.FIFTH_DAY, Days.FIFTH_DAY, Days.SEVENTH_DAY)] //(null,5] [5,7]
    [InlineData(Days.SECOND_DAY, Days.FORTH_DAY, null, Days.FIFTH_DAY)] //[2,4] (null,5]
    //[InlineData(null, null, null, null)]
    public void
        Add_Should_Add_New_Currency_When_There_Is_Overlap_Between_The_Given_Open_Interval_TimePeriod_And_New_One(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(new TimePeriodOptionsTest(timePeriod.from2, timePeriod.to2), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FORTH_DAY, Days.SIXTH_DAY,
        Days.SEVENTH_DAY, Days.NINTH_DAY)] //[1,3] [4,6] [7,9]
    [InlineData(Days.SEVENTH_DAY, Days.NINTH_DAY, Days.FIRST_DAY, Days.THIRD_DAY,
        Days.FORTH_DAY, Days.SIXTH_DAY)] //[7,9] [1,3] [4,6]  
    public void
        Add_Should_Add_New_Currency_When_There_Is_No_Overlap_Between_New_Close_Interval_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .WithTimePeriod(timePeriod.from2, timePeriod.to2)
            .Build();

        currency.Add(new TimePeriodOptionsTest(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.FIRST_DAY,
        Days.THIRD_DAY)] //[1,3] [5,8] [1,3]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.FIFTH_DAY,
        Days.EIGHTH_DAY)] //[1,3] [5,8] [5,8]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.SECOND_DAY,
        Days.THIRD_DAY)] //[1,3] [5,8] [2,3]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.THIRD_DAY,
        Days.FORTH_DAY)] //[1,3] [5,8] [3,4]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.FORTH_DAY,
        Days.FIFTH_DAY)] //[1,3] [5,8] [4,5]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.FIFTH_DAY,
        Days.SIXTH_DAY)] //[1,3] [5,8] [5,6]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.SIXTH_DAY,
        Days.SEVENTH_DAY)] //[1,3] [5,8] [6,7]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.SEVENTH_DAY,
        Days.EIGHTH_DAY)] //[1,3] [5,8] [7,8]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.EIGHTH_DAY,
        Days.NINTH_DAY)] //[1,3] [5,8] [8,9]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY, Days.CURRENT,
        Days.FIRST_DAY)] //[1,3] [5,8] [0,1]
    public void
        Add_Should_Not_Add_New_Currency_When_There_Is_Overlap_Between_New_Close_Interval_TimePeriod_And_Two_Others(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .WithTimePeriod(timePeriod.from2, timePeriod.to2)
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(new TimePeriodOptionsTest(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(2);
    }


    [Theory]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.EIGHTH_DAY,
        null)] //(null,3] [6,7] [8,null]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.FORTH_DAY,
        Days.FIFTH_DAY)] //(null,3] [6,7] [4,5]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.FORTH_DAY,
        Days.FORTH_DAY)] //(null,3] [6,7] [4,4]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.FIFTH_DAY,
        Days.FIFTH_DAY)] //(null,3] [6,7] [5,5]
    public void Add_Should_Add_New_Third_Currency_When_There_Is_No_Overlap_Between_New_TimePeriod_And_Two_Others(
        int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .WithTimePeriod(timePeriod.from2, timePeriod.to2)
            .Build();

        currency.Add(new TimePeriodOptionsTest(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);

        currency.Symbol.Should().Be(_builder.Symbol);
        currency.CurrencyRates.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.SEVENTH_DAY,
        null)] //(null,3] [6,7] [7,null)
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.THIRD_DAY, null)] //(null,3] [6,7] [3,null)
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.FIFTH_DAY,
        Days.SIXTH_DAY)] //(null,3] [6,7] [5,6]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.THIRD_DAY,
        Days.FORTH_DAY)] //(null,3] [6,7] [3,4]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.SECOND_DAY,
        Days.THIRD_DAY)] //(null,3] [6,7] [2,3]
    [InlineData(null, Days.THIRD_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.SEVENTH_DAY,
        Days.EIGHTH_DAY)] //(null,3] [6,7] [7,8]
    public void Add_Should_Add_New_Third_Currency_When_There_Is_Overlap_Between_New_TimePeriod_And_Two_Others(
        int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
        var currency = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .WithTimePeriod(timePeriod.from2, timePeriod.to2)
            .Build();

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            currency.Add(new TimePeriodOptionsTest(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
        currency.CurrencyRates.Should().HaveCount(2);
    }

   //
   //  [Theory]
   //  [InlineData(Days.FIRST_DAY, Days.SECOND_DAY, Days.SECOND_DAY, Days.THIRD_DAY, Days.FORTH_DAY, Days.FIFTH_DAY)]
   //  [InlineData(Days.FIRST_DAY, Days.SECOND_DAY, Days.THIRD_DAY, Days.FORTH_DAY, Days.FORTH_DAY, Days.FIFTH_DAY)]
   // // [InlineData(Days.FIRST_DAY, Days.SECOND_DAY, Days.THIRD_DAY, Days.FORTH_DAY, Days.SECOND_DAY, Days.FIFTH_DAY)]
   //  public void Test1(int? fromDate1, int? toDate1, int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
   //  {
   //      var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2, fromDate3, toDate3);
   //      var currency = _builder
   //          .WithTimePeriod(timePeriod.from1, timePeriod.to1)
   //          .WithTimePeriod(timePeriod.from2, timePeriod.to2)
   //          .Build();
   //
   //      currency.Add(new TimePeriodOptionsTest(timePeriod.from3, timePeriod.to3), CurrencyConsts.SOME_PRICE);
   //
   //      currency.CurrencyRates.Should().HaveCount(3);
   //  }
}