using Triumph.SMS.Remote.Core.Common.Exceptions;
using Triumph.SMS.Remote.Core.Common.ValueObjects;

namespace Triumph.SMS.Remote.Core.Payments.ValueObjects;

public class Money : ValueObjectBase
{
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    
    // For EF
    private Money() { }
    
    private Money(string currency, decimal amount)
    {
        Currency = currency.ToUpper();
        Amount = amount;
    }
    
    public static Money Create(string currency, decimal amount)
    {
        if (string.IsNullOrEmpty(currency) || currency.Length > 3 || amount < 0)
        {
            throw new EntityValidationException("Invalid money value.");
        }
        
        return new Money(currency, amount);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Currency;
        yield return Amount;
    }
}