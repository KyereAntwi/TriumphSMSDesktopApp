using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Students;

namespace Triumph.SMS.Remote.Core.Payments;

public class PaymentHistory : EntityBase<int>
{
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public string PaymentType { get; set; } = string.Empty;
    public string ReceivedBy { get; set; } = string.Empty;

    public string PaidBy { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
