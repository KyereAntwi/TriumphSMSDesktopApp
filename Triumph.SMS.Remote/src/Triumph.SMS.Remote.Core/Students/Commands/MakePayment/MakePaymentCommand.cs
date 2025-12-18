using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Payments.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students.Commands.MakePayment;

public record MakePaymentCommandResult
{
    public IEnumerable<string> Errors { get; set; } = [];
};

public record MakePaymentCommand : ICommand<MakePaymentCommandResult>
{
    public int StudentId { get; set; }
    public Money Amount { get; set; } = null!;
    public string PaidBy { get; set; } = string.Empty;
    public int PaymentTypeId { get; set; }
    public string ReceivedBy { get; set; } = string.Empty;
}
