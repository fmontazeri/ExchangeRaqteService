using FluentAssertions;
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
    public void Construct_New_Currency_Should_Be_Done_Successfully()
    {
        var actual = _builder.BuildOptions();

        _builder.Assert(actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Construct_CurrencyRate_Should_Not_Be_Done_When_Money_Is_Not_Defined(string currency)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var actual = _builder
                .WithMoney(CurrencyConsts.SOME_PRICE, currency)
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(CurrencyIsNotDefinedException.ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Construct_A_New_CurrencyRate_Should_Not_Be_Done_When_The_Amount_Is_Not_Valid(decimal amount)
    {
        var exception = Assert.Throws<AmountIsNotValidException>(() =>
        {
            var actual = _builder
                .WithMoney(amount, CurrencyConsts.SOME_CURRENCY)
                .Build();
        });

        exception.Message.Should().BeEquivalentTo(AmountIsNotValidException.ErrorMessage);
    }


    [Fact]
    public void Construct_CurrencyRate_When_TimePeriod_Set_As_Unlimited_TimePeriod()
    {
        var actual = _builder
            .WithTimePeriod(null, null)
            .Build();

        actual.Should().NotBeNull();
        actual.TimePeriod.FromDate.Should().BeNull();
        actual.TimePeriod.ToDate.Should().BeNull();
    }
}