using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class TimePeriodTests
{
    [Fact]
    public void Constructor_Should_Create_A_TimePeriod_Successfully()
    {
        var actual = TimePeriod.New(DateTime.Today.AddDays(Days.FIRST_DAY), DateTime.Today.AddDays(Days.TENTH_DAY));

        actual.FromDate.Should().Be(DateTime.Today.AddDays(Days.FIRST_DAY));
        actual.ToDate.Should().Be(DateTime.Today.AddDays(Days.TENTH_DAY));
    }

    [Fact]
    public void Constructor_Should_Be_Equal_When_Are_The_Same()
    {
        var first = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(Days.SECOND_DAY));
        var second = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(Days.SECOND_DAY));

        first.Equals(second).Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_Not_Be_Equal_The_Other_Is_Not_Defined()
    {
        var first = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(Days.SECOND_DAY));

        first.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_Not_Be_Equal_When_Are_Not_The_Same()
    {
        var first = TimePeriod.New(DateTime.Today.AddDays(Days.FIRST_DAY), DateTime.Today.AddDays(Days.TENTH_DAY));
        var second = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(Days.SECOND_DAY));

        first.Equals(second).Should().BeFalse();
    }

    [Theory]
    [InlineData(Days.SOME_DAYS , 0)]
    public void Constructor_Should_Not_Be_Created_When_FromDate_Is_After_ToDate(int fromDate , int toDate)
    {
        var exception = Assert.Throws<FromDateIsNotValidException>(() =>
        {
            var first = TimePeriod.New(DateTime.Today.AddDays(fromDate), Days.TODAY);
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }


    [Theory]
    [InlineData(Days.SOME_DAYS-1 , Days.SOME_DAYS)]
    [InlineData(null , Days.SOME_DAYS)]
    [InlineData(Days.SOME_DAYS , null)]
    [InlineData(null , null)]
    public void Constructor_Should_Create_A_New_TimePeriod_When_FromDate_Is_Lower_Than_FromDate(int? fromDate , int? toDate)
    {
        var from = fromDate.HasValue ? DateTime.Today.AddDays(fromDate.Value) : DateTime.MinValue;
        var to = toDate.HasValue ? DateTime.Today.AddDays(toDate.Value) : DateTime.MaxValue;
        var first = TimePeriod.New(from,to);

        first.FromDate.Should().Be(from);
        first.ToDate.Should().Be(to);
    }
    
    [Theory]
    [InlineData(Days.SOME_DAYS-1 , Days.SOME_DAYS ,null , 0)]
    [InlineData(null , Days.SOME_DAYS , Days.SOME_DAYS+1 , null)]
    [InlineData(Days.SOME_DAYS , null , null , 0)]
    public void DoesItOverlapWith_should_Return_False_When_There_Is_No_Overlap_Between_The_Given_And_New_TimePeriod(int? fromDate1 , int? toDate1 , int? fromDate2 , int? toDate2)
    {
        var from1 = fromDate1.HasValue ? DateTime.Today.AddDays(fromDate1.Value) : DateTime.MinValue;
        var to1 = toDate1.HasValue ? DateTime.Today.AddDays(toDate1.Value) : DateTime.MaxValue;
        var first = TimePeriod.New(from1, to1);
        var from2 = fromDate2.HasValue ? DateTime.Today.AddDays(fromDate2.Value) : DateTime.MinValue;
        var to2 = toDate2.HasValue ? DateTime.Today.AddDays(toDate2.Value) : DateTime.MaxValue;
        var second = TimePeriod.New(from2, to2);

        var actual = first.DoesItOverlapWith(second);

        actual.Should().BeFalse();
    }
}