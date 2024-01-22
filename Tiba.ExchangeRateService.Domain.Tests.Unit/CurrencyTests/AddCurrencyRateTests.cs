using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddCurrencyRateTests
{
    private readonly TestCurrencyRateBuilder _builder;

    public AddCurrencyRateTests()
    {
        _builder = new TestCurrencyRateBuilder();
    }

    [Theory]
    [InlineData(TestTimePeriod.FIRST_DAY)]
    [InlineData(TestTimePeriod.SECOND_DAY)]
    public void Constructor_Should_Initial_ExchangeRate_Correctly(int someDays)
    {
        var actual = _builder
            .WithToDate(_builder.FromDate.AddDays(someDays))
            .Build();

        _builder.Assert(actual);
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ExchangeRate_Should_Not_Constructed_When_Currency_Is_Not_Defined(string currencyName)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var actual = _builder
                .WithMoney(new Money(CurrencyConsts.SOME_PRICE, currencyName))
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ExchangeRate_Should_Not_Be_Added_When_Price_Is_Not_Valid(decimal amount)
    {
        var exception = Assert.Throws<PriceIsNotValidException>(() =>
        {
            var actual = _builder
                .WithMoney(new Money(amount , CurrencyConsts.SOME_CURRENCY))
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(PriceIsNotValidException.ErrorMessage);
    }


    [Fact]
    public void ExchangeRate_Should_Not_Be_Constructed_When_FromDate_Is_After_The_ToDate()
    {
        var exception = Assert.Throws<FromDateIsNotValidException>(() =>
        {
            var actual = _builder
                .WithFromDate(TestTimePeriod.TODAY.AddDays(TestTimePeriod.TENTH_DAY))
                .WithToDate(TestTimePeriod.TODAY.AddDays(TestTimePeriod.FIRST_DAY))
                .Build();
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }

    [Fact]
    public void Constructor_Should_Not_Create_Currency_When_FromDate_Is_Not_Defined()
    {
        var actual = Assert.Throws<FromDateIsEmptyException>(() =>
        {
            var currencyRate = _builder
                .WithFromDate(TestTimePeriod.NULL_OR_Default_DATE)
                .Build();
        });

        actual.Message.Should().BeEquivalentTo(FromDateIsEmptyException.ErrorMessage);
    }


    [Fact]
    public void Constructor_Should_Not_Create_Currency_When_ToDate_Is_Not_Defined()
    {
        var actual = Assert.Throws<ToDateIsEmptyException>(() =>
        {
            var currencyRate = _builder
                .WithToDate(TestTimePeriod.NULL_OR_Default_DATE)
                .Build();
        });

        actual.Message.Should().BeEquivalentTo(ToDateIsEmptyException.ErrorMessage);
    }
}