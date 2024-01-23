// using System.Runtime.InteropServices.JavaScript;
// using FluentAssertions;
//
// namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;
//
// public class CurrencyConvertionTests
// {
//     [Fact]
//     public void Constructor_Should_Create_ConversionRate_Successfully()
//     {
//         var actual = new Currency1("USD", "IRR", DateTime.Now, DateTime.Today);
//         actual.Base.Should().Be("USD");
//         actual.ExchangeRates.Should().HaveCount(1);
//         //   actual.ExchangeRates.Should().ContainEquivalentOf(new ExchangeRate1() { })
//     }
// }
//
// public interface ICurrencyOptions
// {
//     public string Base { get; }
// }
//
// public class Currency1 : ICurrencyOptions
// {
//     public string Base { get; private set; }
//     public int Volume { get; private set; } = 1;
//     private List<ExchangeRate1> _exchangeRates = new();
//     public IReadOnlyList<ExchangeRate1> ExchangeRates => _exchangeRates;
//
//     private Currency1(string @base)
//     {
//         this.Base = @base;
//     }
//
//     public Currency1(string @base, string target, DateTime fromDate, DateTime toDate) : this(@base)
//     {
//         this._exchangeRates.Add(new ExchangeRate1()
//         {
//             Amount = GetConversionRate(target),
//             FromDate = fromDate,
//             ToDate = toDate,
//             Target = new Currency1(target)
//         });
//     }
//
//     private decimal GetConversionRate(string target)
//     {
//         //TODO: check volum
//         return (decimal)(this.Volume * ConversionRates.ExchangeRates[this.Base][target]);
//     }
// }
//
// public class ExchangeRate1
// {
//     public DateTime FromDate { get; set; }
//     public DateTime ToDate { get; set; }
//     public ICurrencyOptions Target { get; set; }
//     public decimal Amount { get; set; }
// }
//
// public class ConversionRates
// {
//     public static Dictionary<string, Dictionary<string, double>> ExchangeRates = new()
//     {
//         { "USD", new Dictionary<string, double>() { { "IRR", 0.000024 } } },
//         { "IRR", new Dictionary<string, double>() { { "USD", 42050 } } }
//     };
// }