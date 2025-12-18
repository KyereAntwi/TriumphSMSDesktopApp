using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Payments.Queries;

public record PaymentTypeDto(int Id, string Type);
public record PaymentTypesResult(IEnumerable<PaymentTypeDto> PaymentTypes)
{
    public IEnumerable<string> Errors { get; set; } = [];
};

public record GetAllPaymentTypesQuery : IQuery<PaymentTypesResult>;
public sealed class GetAllPaymentTypesHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAllPaymentTypesQuery, PaymentTypesResult>
{
    public async Task<PaymentTypesResult> Handle(GetAllPaymentTypesQuery request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<PaymentTypesResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new PaymentTypesResult([]) 
            {
                Errors = [$"There was an error fetching payment types. Error = {ex.Message}"]
            });
    }

    private async Task<PaymentTypesResult> HandleAsync(GetAllPaymentTypesQuery query, CancellationToken cancellationToken)
    {
        var list = await dbContext
            .PaymentTypes
            .Select(pt => new PaymentTypeDto(pt.Id, pt.Type))
            .ToListAsync(cancellationToken);

        return new PaymentTypesResult(list);
    }
}
