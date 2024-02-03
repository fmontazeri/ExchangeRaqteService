using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddCurrencyRateTests
{
    private readonly CurrencyRateTestBuilder _builder;

    public AddCurrencyRateTests()
    {
        _builder = new CurrencyRateTestBuilder();
    }

    [Fact]
    public void Constructor_Should_Create_A_CurrencyRate_Correctly()
    {
        var actual = _builder.Build();

        _builder.Assert(actual);
    }

    [Fact]
    public void ExchangeRate_Should_Not_Constructed_When_Money_Is_Not_Defined()
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var actual = _builder
                .WithMoney(null)
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ExchangeRate_Should_Not_Be_Added_When_Price_Is_Not_Valid(decimal amount)
    {
        var exception = Assert.Throws<AmountIsNotValidException>(() =>
        {
            var actual = _builder
                .WithMoney(CurrencyAgg.Money.New(amount, CurrencyConsts.SOME_CURRENCY))
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(AmountIsNotValidException.ErrorMessage);
    }

}