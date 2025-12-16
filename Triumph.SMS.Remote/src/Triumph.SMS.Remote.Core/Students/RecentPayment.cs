using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Payments;
using Triumph.SMS.Remote.Core.Payments.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students;

public class RecentPayment : EntityBase<Guid>
{
    // For EF
    private RecentPayment() { }
    
    private RecentPayment(int studentId, Money amount, string paidBy, PaymentType paymentType)
    {
        Id = Guid.CreateVersion7();
        StudentId = studentId;
        Amount = amount;
        PaidBy = paidBy;
        PaymentType = paymentType;
    }
    
    public static RecentPayment Create(int studentId, Money amount, string paidBy, PaymentType paymentType)
    {
        return new RecentPayment(studentId, amount, paidBy, paymentType);
    }
    
    public int StudentId { get; private set; }
    public Student? Student { get; private set; }
    
    public Money Amount { get; private set; } = null!;
    public string PaidBy { get; private set; } = string.Empty;
    public PaymentType PaymentType { get; private set; } = null!;
}