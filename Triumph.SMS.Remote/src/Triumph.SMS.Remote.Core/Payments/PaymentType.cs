using Triumph.SMS.Remote.Core.Common.Entities;

namespace Triumph.SMS.Remote.Core.Payments;

public class PaymentType : EntityBase<int>
{
    // for EF
    private PaymentType() { }

    private PaymentType(string type)
    {
        Type = type;
    }

    public PaymentType Create(string type) => new PaymentType(type);
    
    public string Type { get; private set; } = string.Empty;
}