using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Reports.Queries;

public record StudentDto(
    int Id,
    string FullName);

public record PaymentHistoryReportDto(
    StudentDto Student,
    decimal Amount,
    string Currency,
    string PaidBy,
    string PaymentType,
    DateTime CreatedAt,
    string ReceivedBy);

public record PaymentHistoryReportResult
{
    public IEnumerable<PaymentHistoryReportDto> PaymentHistoryItems { get; set; } = [];
    public int TotalCount { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
}

public record PaymentHistoryReportQuery(
    string PaidFrom = "",
    string PaidTo = "",
    DateTime PaymentFrom = default,
    DateTime PaymentTo = default,
    string PaymentType = "",
    int Page = 1,
    int PageSize = 50) 
    : IQuery<PaymentHistoryReportResult>;

public sealed class PaymentHistoryReportQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<PaymentHistoryReportQuery, PaymentHistoryReportResult>
{
    public async Task<PaymentHistoryReportResult> Handle(PaymentHistoryReportQuery query, CancellationToken cancellationToken)
    {
        return await ((Func<Task<PaymentHistoryReportResult>>)(() => HandleAsync(query, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new PaymentHistoryReportResult
            {
                Errors = ["An error occurred while processing the payment history report request.", ex.Message]
            });
    }

    private async Task<PaymentHistoryReportResult> HandleAsync(PaymentHistoryReportQuery query, CancellationToken cancellationToken)
    {
        var innerQuery = dbContext
            .PaymentHistories
            .Include(ph => ph.Student)
            .Include(ph => ph.PaymentType)
            .AsSplitQuery()
            .AsQueryable();

        innerQuery = FilterQueryHistory(query, innerQuery);

        var totalCount = await innerQuery.CountAsync(cancellationToken);

        var pagedResult = await innerQuery
            .OrderBy(ph => ph.StudentId)
            .OrderByDescending(ph => ph.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PaymentHistoryReportResult
        {
            PaymentHistoryItems = pagedResult.Select(ph => new PaymentHistoryReportDto(
                new StudentDto(ph.Student!.Id, ph.Student.ToString()),
                ph.Amount,
                ph.Currency,
                ph.PaidBy,
                ph.PaymentType,
                ph.CreatedAt,
                ph.ReceivedBy)),
            TotalCount = totalCount
        };
    }

    private static IQueryable<Payments.PaymentHistory> FilterQueryHistory(PaymentHistoryReportQuery query, IQueryable<Payments.PaymentHistory> innerQuery)
    {
        if (!string.IsNullOrWhiteSpace(query.PaidFrom))
        {
            innerQuery = innerQuery.Where(ph => ph.ReceivedBy.ToLower().Contains(query.PaidFrom.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.PaidTo))
        {
            innerQuery = innerQuery.Where(ph => ph.PaidBy.ToLower().Contains(query.PaidTo.ToLower()));
        }

        if (query.PaymentFrom != default)
        {
            innerQuery = innerQuery.Where(ph => ph.CreatedAt >= query.PaymentFrom);
        }

        if (query.PaymentTo != default)
        {
            innerQuery = innerQuery.Where(ph => ph.CreatedAt <= query.PaymentTo);
        }

        if (!string.IsNullOrWhiteSpace(query.PaymentType))
        {
            innerQuery = innerQuery.Where(ph => ph.PaymentType.Contains(query.PaymentType));
        }

        return innerQuery;
    }
}
