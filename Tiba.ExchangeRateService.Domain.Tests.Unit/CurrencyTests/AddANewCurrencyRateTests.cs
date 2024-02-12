using System.Diagnostics.Contracts;
using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class AddANewCurrencyRateTests
{
    private CurrencyTestBuilder _builder;
    private Currency _currency;

    public AddANewCurrencyRateTests()
    {
        _builder = new CurrencyTestBuilder();
    }


    #region Happy paths

    [Theory]
    [InlineData(null, Days.SOME_DAYS)]
    [InlineData(null, null)]
    [InlineData(Days.SOME_DAYS, null)]
    [InlineData(Days.SOME_DAYS, Days.SOME_DAYS + 1)]
    public void Constructor_Should_Add_A_New_CurrencyRate_Successfully(int? fromDate1, int? toDate1)
    {
        var timePeriod = GetTimePeriod(fromDate1, toDate1);
        var sut = _builder.WithTimePeriod(timePeriod.fromDate, timePeriod.toDate);

        _currency = sut.Build();

        _currency.CurrencyRates.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(0, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2)]
    [InlineData(null, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2)]
    [InlineData(null, Days.SOME_DAYS, Days.SOME_DAYS + 1, null)]
    [InlineData(0, Days.SOME_DAYS, Days.SOME_DAYS + 1, null)]
    [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, 0, Days.SOME_DAYS)]
    [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, null, Days.SOME_DAYS)]
    [InlineData(Days.SOME_DAYS + 1, null, null, Days.SOME_DAYS)]
    [InlineData(Days.SOME_DAYS + 1, null, 0, Days.SOME_DAYS)]
    public void Add_Should_Add_Second_TimePeriod_With_No_TimePeriod_Overlap_Successfully(int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        Constructor_Should_Add_A_New_CurrencyRate_Successfully(fromDate1, toDate1);
        var timePeriod = GetTimePeriod(fromDate2, toDate2);

        _currency.Add(new TimePeriodOptionsTest(timePeriod.fromDate, timePeriod.toDate), CurrencyConsts.SOME_PRICE);

        _currency.CurrencyRates.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(0, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, Days.SOME_DAYS + 3, Days.SOME_DAYS + 4)]
    [InlineData(null, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, Days.SOME_DAYS + 3, Days.SOME_DAYS + 4)]
    [InlineData(Days.SOME_DAYS + 3, null, Days.SOME_DAYS - 1, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS, null, 0, Days.SOME_DAYS + 1, Days.SOME_DAYS + 3)]
    [InlineData(Days.SOME_DAYS - 3, 0, Days.SOME_DAYS + 1, null, Days.SOME_DAYS - 1, Days.SOME_DAYS)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, null, 0)]
    [InlineData(Days.SOME_DAYS - 3, 0, Days.SOME_DAYS - 1, Days.SOME_DAYS, Days.SOME_DAYS + 1, null)]
    [InlineData(null, 0, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, Days.SOME_DAYS + 3, null)]
    [InlineData(null, 0, Days.SOME_DAYS + 1, null, Days.SOME_DAYS - 1, Days.SOME_DAYS)]
    [InlineData(Days.SOME_DAYS + 1, null, null, -1, Days.SOME_DAYS - 1, Days.SOME_DAYS)]
    [InlineData(0, Days.SOME_DAYS, null, -1, Days.SOME_DAYS + 1, null)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS, Days.SOME_DAYS + 1, null, null, 0)]
    public void Add_Should_Add_Third_CurrencyRate_With_No_TimePeriod_Overlap_Successfully(int? fromDate1, int? toDate1,
        int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        Add_Should_Add_Second_TimePeriod_With_No_TimePeriod_Overlap_Successfully(fromDate1, toDate1, fromDate2,
            toDate2);
        var timePeriod = GetTimePeriod(fromDate3, toDate3);

        _currency.Add(new TimePeriodOptionsTest(timePeriod.fromDate, timePeriod.toDate), CurrencyConsts.SOME_PRICE);

        _currency.CurrencyRates.Should().HaveCount(3);
    }

    #endregion


    #region Exception paths

    [Fact]
    public void Constructor_Should_Not_Add_A_New_CurrencyRate_When_FromDate_Is_After_ToDate()
    {
        var sut = _builder.WithTimePeriod(DateTime.Today.AddDays(Days.SOME_DAYS), DateTime.Today);

        var exception = Assert.Throws<FromDateIsNotValidException>(() => { _currency = sut.Build(); });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }

    [Theory]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS + 2, Days.SOME_DAYS + 1, Days.SOME_DAYS + 3)]
    [InlineData(Days.SOME_DAYS - 1, null, Days.SOME_DAYS + 1, Days.SOME_DAYS + 3)]
    [InlineData(null, Days.SOME_DAYS + 2, Days.SOME_DAYS + 1, Days.SOME_DAYS + 3)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS + 2, Days.SOME_DAYS + 1, null)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS + 2, null, Days.SOME_DAYS + 3)]
    [InlineData(null, Days.SOME_DAYS + 2, Days.SOME_DAYS + 1, null)]
    [InlineData(null, null, Days.SOME_DAYS + 1, Days.SOME_DAYS + 3)]
    [InlineData(null, Days.SOME_DAYS + 2, null, Days.SOME_DAYS + 3)]
    [InlineData(Days.SOME_DAYS - 1, null, null, Days.SOME_DAYS + 3)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS + 2, null, null)]
    [InlineData(Days.SOME_DAYS - 1, null, Days.SOME_DAYS + 1, null)]
    [InlineData(null, null, null, Days.SOME_DAYS + 3)]
    [InlineData(null, null, Days.SOME_DAYS + 1, null)]
    [InlineData(Days.SOME_DAYS - 1, null, null, null)]
    [InlineData(null, null, null, null)]
    //---
    [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 3, Days.SOME_DAYS - 1, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 3, null, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS + 1, null, Days.SOME_DAYS - 1, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 3, Days.SOME_DAYS - 1, null)]
    [InlineData(null, Days.SOME_DAYS + 3, Days.SOME_DAYS - 1, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS + 1, null, null, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 3, null, null)]
    [InlineData(null, Days.SOME_DAYS + 3, null, Days.SOME_DAYS + 2)]
    [InlineData(null, Days.SOME_DAYS + 3, Days.SOME_DAYS - 1, null)]
    [InlineData(null, null, Days.SOME_DAYS - 1, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS + 1, null, Days.SOME_DAYS - 1, null)]
    //--
    [InlineData(null, Days.SOME_DAYS + 3, null, null)]
    [InlineData(Days.SOME_DAYS + 1, null, null, null)]
    [InlineData(null, null, null, Days.SOME_DAYS + 2)]
    [InlineData(null, null, Days.SOME_DAYS - 1, null)]

    // [InlineData(0, Days.SOME_DAYS + 1, Days.SOME_DAYS + 1, Days.SOME_DAYS + 3)]
    // [InlineData(null, Days.SOME_DAYS + 1, Days.SOME_DAYS, Days.SOME_DAYS + 3)]
    // [InlineData(0, null, Days.SOME_DAYS, Days.SOME_DAYS + 3)]
    // [InlineData(0, Days.SOME_DAYS, null, Days.SOME_DAYS + 3)]
    // [InlineData(0, Days.SOME_DAYS + 1, Days.SOME_DAYS, null)]
    // [InlineData(Days.SOME_DAYS + 1, Days.SOME_DAYS + 3, 0, Days.SOME_DAYS + 1)]
    // [InlineData(null, null, 0, Days.FIRST_DAY)]
    // [InlineData(null, 0, null, Days.FIRST_DAY)]
    // [InlineData(0, null, Days.SOME_DAYS + 1, null)]
    // [InlineData(null, Days.SOME_DAYS, Days.SOME_DAYS - 1, null)]
    public void Add_Should_Not_Add_A_New_CurrencyRate_When_There_Is_Overlap_Between_The_Given_TimePeriod_And_New_One(
        int? fromDate1, int? toDate1, int? fromDate2, int? toDate2)
    {
        Constructor_Should_Add_A_New_CurrencyRate_Successfully(fromDate1, toDate1);
        var timePeriod = GetTimePeriod(fromDate2, toDate2);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            _currency.Add(new TimePeriodOptionsTest(timePeriod.fromDate, timePeriod.toDate),
                CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }


    [Theory]
    [InlineData(0, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, Days.SOME_DAYS + 2, Days.SOME_DAYS + 3)]
    [InlineData(0, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, null, Days.SOME_DAYS)]
    [InlineData(0, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, Days.SOME_DAYS - 1, null)]
    [InlineData(null, Days.SOME_DAYS - 1, Days.SOME_DAYS, Days.SOME_DAYS + 1, 0, null)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS, null, 0, Days.SOME_DAYS - 3, null)]
    [InlineData(null, 0, Days.SOME_DAYS - 1, null, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2)]
    [InlineData(Days.SOME_DAYS - 1, Days.SOME_DAYS, Days.SOME_DAYS + 1, Days.SOME_DAYS + 2, null, null)]
    public void Add_Should_Not_Add_Third_CurrencyRate_When_There_Is_TimePeriod_Overlap(int? fromDate1, int? toDate1,
        int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        Add_Should_Add_Second_TimePeriod_With_No_TimePeriod_Overlap_Successfully(fromDate1, toDate1, fromDate2,
            toDate2);
        var timePeriod = GetTimePeriod(fromDate3, toDate3);

        var exception = Assert.Throws<OverlapTimePeriodException>(() =>
        {
            _currency.Add(new TimePeriodOptionsTest(timePeriod.fromDate, timePeriod.toDate),
                CurrencyConsts.SOME_PRICE);
        });

        exception.Message.Should().Be(OverlapTimePeriodException.ErrorMessage);
    }

    #endregion

    private (DateTime? fromDate, DateTime? toDate) GetTimePeriod(int? fromDate1, int? toDate1)
    {
        DateTime? fromDate = fromDate1.HasValue ? Days.TODAY.AddDays(fromDate1.Value) : null;
        DateTime? toDate = toDate1.HasValue ? Days.TODAY.AddDays(toDate1.Value) : null;

        return (fromDate, toDate);
    }
}