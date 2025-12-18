using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;
using Triumph.SMS.Remote.Core.Payments.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students.Commands.MakePayment;

public sealed class MakePaymentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<MakePaymentCommand, MakePaymentCommandResult>
{
    public async Task<MakePaymentCommandResult> Handle(MakePaymentCommand command, CancellationToken cancellationToken)
    {
        return await ((Func<Task<MakePaymentCommandResult>>)(() => HandleAsync(command, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new MakePaymentCommandResult
            {
                Errors = [$"There was a problem when making payment for student. Error = ${ex.Message}"]
            });
    }

    private async Task<MakePaymentCommandResult> HandleAsync(MakePaymentCommand command, CancellationToken cancellationToken)
    {
        var paymentType = await dbContext
            .PaymentTypes
            .FindAsync([command.PaymentTypeId], cancellationToken);

        if(paymentType is null)
        {
            return new MakePaymentCommandResult
            {
                Errors = [$"Payment type with Id {command.PaymentTypeId} does not exist."]
            };
        }

        var existingStudent = await dbContext
            .Students
            .Include(s => s.RecentPayment)
            .AsSplitQuery()
            .AsTracking()
            .Where(s => s.Id == command.StudentId)
            .FirstOrDefaultAsync(cancellationToken);

        if(existingStudent is null)
        {
            return new MakePaymentCommandResult
            {
                Errors = [$"Student with Id {command.StudentId} does not exist."]
            };
        }

        var moneyAmount = Money.Create(command.Amount.Currency, command.Amount.Amount);
        var recentPayment = RecentPayment.Create(
                command.StudentId,
                moneyAmount,
                command.PaidBy,
                paymentType,
                command.ReceivedBy);

        existingStudent.MakePayment(recentPayment, false);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new MakePaymentCommandResult();
    }
}
