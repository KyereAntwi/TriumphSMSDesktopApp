using Triumph.SMS.Remote.Core.Common.CQRS;

namespace Triumph.SMS.Remote.Core.Payments.Commands.AddPaymentType;

public record AddPaymentTypeResult
{
    public IEnumerable<string> Errors { get; set; } = [];
}


public record AddPaymentTypeCommand : ICommand<AddPaymentTypeResult>
{
    public string Type { get; set; } = string.Empty;
}
