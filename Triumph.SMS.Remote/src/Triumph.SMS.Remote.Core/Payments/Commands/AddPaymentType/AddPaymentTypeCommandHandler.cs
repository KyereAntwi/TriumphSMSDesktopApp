using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Payments.Commands.AddPaymentType;

public sealed class AddPaymentTypeCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<AddPaymentTypeCommand, AddPaymentTypeResult>
{
    public async Task<AddPaymentTypeResult> Handle(AddPaymentTypeCommand request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<AddPaymentTypeResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new AddPaymentTypeResult
            {
                Errors = [$"There was an error processing the request: {ex.Message}"]
            });
    }

    private async Task<AddPaymentTypeResult> HandleAsync(AddPaymentTypeCommand command, CancellationToken cancellationToken)
    {
        var existingType = await dbContext
            .PaymentTypes
            .Where(pt => pt.Type == command.Type)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingType is not null)
        {
            return new AddPaymentTypeResult
            {
                Errors = [$"A payment type with the name '{command.Type}' already exists."]
            };
        }

        var newPaymentType = PaymentType.Create(command.Type);
        await dbContext.PaymentTypes.AddAsync(newPaymentType);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddPaymentTypeResult();
    }
}
