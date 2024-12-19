using System.Globalization;

namespace CompanyName.Services.ProductService.Domain.Products.Fields;

public readonly record struct Money
{
    private Money(decimal value, Currency currency) => (Value, Currency) = (value, currency);
    public readonly decimal Value { get; }
    public readonly Currency Currency { get; }
    public static Money Zero(Currency currency) => new(0, currency);
    public static Money From(decimal value, Currency currency) => new(value, currency);

    public static Money operator +(Money x, Money y) => Calculate(x, y, (a, b) => a + b);
    public static Money operator -(Money x, Money y) => Calculate(x, y, (a, b) => a - b);


    public static Money operator *(Money x, int y) => Calculate(x, y, (a, b) => a * b);
    public static Money operator /(Money x, int y) => Calculate(x, y, (a, b) => a / b);
    public static Money operator *(Money x, decimal y) => Calculate(x, y, (a, b) => a * b);
    public static Money operator /(Money x, decimal y) => Calculate(x, y, (a, b) => a / b);

    public static Money Max(Money x, Money y) => x > y ? x : y;

    public static bool operator >(Money x, Money y) => Compare(x, y, (a, b) => a > b);
    public static bool operator <(Money x, Money y) => Compare(x, y, (a, b) => a < b);
    public static bool operator >=(Money x, Money y) => Compare(x, y, (a, b) => a >= b);
    public static bool operator <=(Money x, Money y) => Compare(x, y, (a, b) => a <= b);

    private static bool Compare(Money x, Money y, Func<decimal, decimal, bool> compare) => compare(x.Value, y.Value);
    private static Money Calculate<T>(Money x, T y, Func<decimal, T, decimal> calculate) => new(calculate(x.Value, y), x.Currency);
    private static Money Calculate(Money x, Money y, Func<decimal, decimal, decimal> calculate) => new(calculate(x.Value, y.Value), x.Currency);

    public override string ToString() =>
        $"{Value.ToString("F", CultureInfo.InvariantCulture)} {Currency}";
}
