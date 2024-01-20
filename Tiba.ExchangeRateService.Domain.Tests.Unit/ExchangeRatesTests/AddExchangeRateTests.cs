using FluentAssertions;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.ExchangeRatesTests;

public class AddExchangeRateTests
{
    [Theory]
    [InlineData(1, 200)]
    [InlineData(3, 450)]
    [InlineData(-4, 450)]
    public void Constructor_Should_Initial_ExchangeRate_Correctly(int theDayAfterTomorrow, decimal price)
    {
        var today = DateTime.Today;
        var nextDay = today.AddDays(theDayAfterTomorrow);
        var actual = new ExchangeRate(today, nextDay, price);

        actual.FromDateDate.Date.Should().Be(today.Date);
        actual.ToDateDate.Date.Should().Be(nextDay.Date);
        actual.Price.Should().Be(price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(-120000)]
    public void ExchangeRate_Should_Not_Be_Created_When_Price_Is_Not_Valid(decimal price)
    {
        var exception = Assert.Throws<PriceIsNotValidException>(() =>
        {
            var actual = new ExchangeRate(DateTime.Now, DateTime.Now, price);
        });

        exception.Message.Should().BeEquivalentTo(PriceIsNotValidException.ErrorMessage);
    }

    [Fact]
    public void ExchangeRate_Should_Not_Be_Created_When_FromDate_Is_LowerThan_ToDate()
    {
        var today = DateTime.Today;
        var previousDay = today.AddDays(-1);

        var exception = Assert.Throws<FromDateIsNotValidException>(() =>
        {
            var actual = new ExchangeRate(previousDay, today, 120000);
        });

        exception.Message.Should().Be(FromDateIsNotValidException.ErrorMessage);
    }
}

public class FromDateIsNotValidException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "FromDate should be greater equal than ToDate.";
}

public class PriceIsNotValidException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "The Price should be greater than zero.";
}

public class ExchangeRate
{
    public ExchangeRate(DateTime fromDate, DateTime toDate, decimal price)
    {
        if (price <= 0)
            throw new PriceIsNotValidException();

        if (fromDate < toDate)
            throw new FromDateIsNotValidException();
        
        this.FromDateDate = fromDate;
        this.ToDateDate = toDate;
        this.Price = price;
    }


    public DateTime FromDateDate { get; private set; }
    public DateTime ToDateDate { get; private set; }
    public decimal Price { get; private set; }
}