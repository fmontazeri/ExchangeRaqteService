using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class TimePeriodTests
{
    [Fact]
    public void Constructor_Should_Create_A_TimePeriod_Successfully()
    {
        var actual = TimePeriod.New(DateTime.Today.AddDays(DayConsts.FIRST_DAY),
            DateTime.Today.AddDays(DayConsts.TENTH_DAY));

        actual.FromDate.Should().Be(DateTime.Today.AddDays(DayConsts.FIRST_DAY));
        actual.ToDate.Should().Be(DateTime.Today.AddDays(DayConsts.TENTH_DAY));
    }

    [Fact]
    public void Constructor_Should_Be_Equal_When_Are_The_Same()
    {
        var first = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(DayConsts.SECOND_DAY));
        var second = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(DayConsts.SECOND_DAY));

        first.Equals(second).Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_Not_Be_Equal_The_Other_Is_Not_Defined()
    {
        var first = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(DayConsts.SECOND_DAY));

        first.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_Not_Be_Equal_When_Are_Not_The_Same()
    {
        var first = TimePeriod.New(DateTime.Today.AddDays(DayConsts.FIRST_DAY),
            DateTime.Today.AddDays(DayConsts.TENTH_DAY));
        var second = TimePeriod.New(DateTime.Today, DateTime.Today.AddDays(DayConsts.SECOND_DAY));

        first.Equals(second).Should().BeFalse();
    }
    
    [Fact]
    public void Constructor_Should_Not_Be_Created_When_FromDate_Is_After_ToDate()
    {
        var exception = Assert.Throws<FromDateIsNotValidException>(() =>
        {
            var first = TimePeriod.New(DayConsts.TODAY.AddDays(DayConsts.NINTH_DAY), DayConsts.TODAY);
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }
}