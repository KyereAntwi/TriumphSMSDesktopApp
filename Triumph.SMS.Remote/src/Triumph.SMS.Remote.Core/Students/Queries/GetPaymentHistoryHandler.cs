using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Queries;

public record PaymentHistoryItem(
    decimal Amount,
    string Currency,
    string PaidBy,
    string PaymentType,
    DateTime CreatedAt,
    string ReceivedBy);
    
public record GetPaymentHistoryResult(IEnumerable<PaymentHistoryItem> Items)
{
    public IEnumerable<string> Errors { get; set; } = [];
};

public record GetPaymentHistoryQuery(int StudentId, int Page = 1, int PageSize = 50) : IQuery<GetPaymentHistoryResult>;

public sealed class GetPaymentHistoryQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPaymentHistoryQuery, GetPaymentHistoryResult>
{
    public async Task<GetPaymentHistoryResult> Handle(GetPaymentHistoryQuery request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetPaymentHistoryResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex =>
                new GetPaymentHistoryResult([])
            {
                Errors = [$"An error occurred while retrieving payment history: {ex.Message}"]
            });
    }
    
    private async Task<GetPaymentHistoryResult> HandleAsync(GetPaymentHistoryQuery query, CancellationToken cancellationToken)
    {
        var paymentHistoryItems = await dbContext
            .PaymentHistories
            .OrderByDescending(ph => ph.CreatedAt)
            .Where(ph => ph.StudentId == query.StudentId)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(ph => new PaymentHistoryItem(
                ph.Amount,
                ph.Currency,
                ph.PaidBy,
                ph.PaymentType,
                ph.CreatedAt,
                ph.ReceivedBy))
            .ToListAsync(cancellationToken);
        
        return new GetPaymentHistoryResult(paymentHistoryItems);
    }
}