using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class ConstructCurrencyTests : BaseCurrencyTests
{
    private readonly CurrencyTestBuilder _builder;

    public ConstructCurrencyTests()
    {
        _builder = new CurrencyTestBuilder();
    }

    [Fact]
    public void Construct_Should_Create_A_New_Currency_With_A_TimePeriod_Successfully()
    {
        var actual = _builder
            .WithTimePeriod(Days.TODAY, Days.TODAY.AddDays(Days.SOME_DAYS))
            .Build();
        
        actual.CurrencyRates.Should().HaveCount(1);
        actual.Should().BeEquivalentTo(_builder);
    }
    
    [Fact]
    public void Currency_Should_Be_Construct_When_CurrencyRate_Includes_An_Unlimited_TimePeriod()
    {
        var actual = _builder
            .WithTimePeriod(null, null)
            .Build();

        actual.CurrencyRates.Should().HaveCount(1);
        actual.Should().BeEquivalentTo(_builder);
    }

    [Theory]
    [InlineData(null, Days.THIRD_DAY, Days.FORTH_DAY, Days.SIXTH_DAY)] //(null,3] [4,6]
    [InlineData(Days.FORTH_DAY, Days.SIXTH_DAY, null, Days.THIRD_DAY)] //[4,6] (null,3] 
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FORTH_DAY, null)] //[1,3] [4,null)
    [InlineData(Days.FORTH_DAY, null, Days.FIRST_DAY, Days.THIRD_DAY)] //[4,null) [1,3] 
    public void Constructor_Should_Construct_Currency_When_There_Is_No_Overlap_Between_The_Two_Given_Open_Interval_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

        var actual = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .WithTimePeriod(timePeriod.from2, timePeriod.to2)
            .Build();

        actual.CurrencyRates.Should().HaveCount(2);
        actual.Should().BeEquivalentTo(_builder);
    }

    [Theory]
    [InlineData(null, Days.THIRD_DAY, Days.THIRD_DAY, Days.SIXTH_DAY)] //(null,3] [3,6]
    [InlineData(Days.FIFTH_DAY, null, Days.THIRD_DAY, Days.SIXTH_DAY)] //[1,null) [3,6]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, null, Days.SIXTH_DAY)] //[1,3] (null,6] 
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.THIRD_DAY, null)] //[1,3] [3,null)
    [InlineData(null, null, null, null)]
    public void Constructor_Should_Construct_Currency_When_There_Is_Overlap_Between_The_Two_Open_Interval_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var actual = _builder
                .WithTimePeriod(timePeriod.from1, timePeriod.to1)
                .WithTimePeriod(timePeriod.from2, timePeriod.to2)              
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }

    [Theory]
    [InlineData(Days.FIRST_DAY, Days.THIRD_DAY, Days.FORTH_DAY, Days.SIXTH_DAY)]    //[1,3] [4,6]
    [InlineData(Days.SECOND_DAY, Days.FORTH_DAY, Days.FIFTH_DAY, Days.SEVENTH_DAY)] //[2,4] [5,7]
    [InlineData(Days.THIRD_DAY, Days.FIFTH_DAY, Days.SIXTH_DAY, Days.EIGHTH_DAY)]   //[3,5] [6,8]
    [InlineData(Days.FORTH_DAY, Days.SIXTH_DAY, Days.SEVENTH_DAY, Days.NINTH_DAY)]  //[4,6] [7,9]
    public void Construct_A_New_Currency_Should_Be_Done_When_There_Is_No_Overlap_Between_The_Two_Given_Close_Interval_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        
        var actual = _builder
            .WithTimePeriod(timePeriod.from1, timePeriod.to1)
            .WithTimePeriod(timePeriod.from2, timePeriod.to2)
            .Build();

        actual.CurrencyRates.Should().HaveCount(2);
        actual.Should().BeEquivalentTo(_builder);
    }

    [Theory]
    [InlineData(Days.FIRST_DAY, Days.FORTH_DAY, Days.FIRST_DAY, Days.FORTH_DAY)]    //[1,4] [1,4]
    [InlineData(Days.FIRST_DAY, Days.FORTH_DAY, Days.SECOND_DAY, Days.FIFTH_DAY)]   //[1,4] [2,5]
    [InlineData(Days.SECOND_DAY, Days.FIFTH_DAY, Days.THIRD_DAY, Days.SIXTH_DAY)]   //[2,5] [3,6]
    [InlineData(Days.THIRD_DAY, Days.SIXTH_DAY, Days.FORTH_DAY, Days.SEVENTH_DAY)]  //[3,6] [4,7]
    [InlineData(Days.FORTH_DAY, Days.SEVENTH_DAY, Days.FIFTH_DAY, Days.EIGHTH_DAY)] //[4,7] [5,8]
    public void Construct_A_New_Currency_Should_Not_Be_Done_When_There_Is_Overlap_Between_The_Two_Given_Close_Interval_TimePeriods(
            int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1, fromDate2, toDate2);
        
        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            var actual = _builder
                .WithTimePeriod(timePeriod.from1, timePeriod.to1)
                .WithTimePeriod(timePeriod.from2, timePeriod.to2)
                .Build();
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }

}