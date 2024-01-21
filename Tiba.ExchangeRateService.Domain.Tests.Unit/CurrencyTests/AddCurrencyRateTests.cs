using FluentAssertions;
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
    [InlineData(1, 200)]
    [InlineData(3, 450)]
    public void Constructor_Should_Initial_ExchangeRate_Correctly(int theDayAfterTomorrow, decimal price)
    { 
        var actual = _builder
            .WithToDate(_builder.FromDate.AddDays(theDayAfterTomorrow))
            .WithPrice(price)
            .Build();

        _builder.Assert(actual);
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ExchangeRate_Should_Not_Constructed_When_Currency_Is_Not_Defined(string currency)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var actual = _builder
                .WithCurrency(currency)
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(-120000)]
    public void ExchangeRate_Should_Not_Be_Added_When_Price_Is_Not_Valid(decimal price)
    {
        var exception = Assert.Throws<PriceIsNotValidException>(() =>
        {
            var actual = _builder
                .WithPrice(price)
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
                .WithFromDate(DateTime.Today.AddDays(CurrencyRateConsts.SOME_DAYS))
                .WithToDate(DateTime.Today)
                .Build();
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }
}