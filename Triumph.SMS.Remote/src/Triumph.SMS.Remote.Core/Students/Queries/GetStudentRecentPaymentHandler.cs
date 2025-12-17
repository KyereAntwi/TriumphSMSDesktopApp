using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Queries;

public record GetStudentRecentPaymentQueryResult(
    decimal Amount,
    string Currency,
    string PaidBy,
    string PaymentType,
    DateTime CreatedAt,
    string ReceivedBy)
{
    public IEnumerable<string> Errors { get; set; } = [];
};

public record GetStudentRecentPaymentQuery(int Id) : IQuery<GetStudentRecentPaymentQueryResult>;
    
public sealed class GetStudentRecentPaymentQueryHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<GetStudentRecentPaymentQuery, GetStudentRecentPaymentQueryResult>
{
    public async Task<GetStudentRecentPaymentQueryResult> Handle(GetStudentRecentPaymentQuery query, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetStudentRecentPaymentQueryResult>>) (() => HandleAsync(query, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => 
                new GetStudentRecentPaymentQueryResult(0, string.Empty, string.Empty, string.Empty, new DateTime(), string.Empty)
            {
                Errors = [$"An error occurred while retrieving recent payment: {ex.Message}"]
            });
    }
    
    private async Task<GetStudentRecentPaymentQueryResult> HandleAsync(GetStudentRecentPaymentQuery query, CancellationToken cancellationToken)
    {
        var student = await dbContext
            .Students
            .Include(s => s.RecentPayment)
            .ThenInclude(rp => rp!.PaymentType)
            .AsSplitQuery()
            .Where(s => s.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (student is null)
        {
            return new GetStudentRecentPaymentQueryResult(0, string.Empty, string.Empty, string.Empty, new DateTime(), string.Empty)
            {
                Errors = ["Student not found."]
            };
        }
        
        if (student.RecentPayment is null)
        {
            return new GetStudentRecentPaymentQueryResult(0, string.Empty,string.Empty, string.Empty, new DateTime(), string.Empty)
            {
                Errors = ["No recent payment found for the student."]
            };
        }
        
        return new GetStudentRecentPaymentQueryResult(
            student.RecentPayment.Amount.Amount,
            student.RecentPayment.Amount.Currency,
            student.RecentPayment.PaidBy,
            student.RecentPayment.PaymentType.Type,
            student.RecentPayment.CreatedAt,
            student.RecentPayment.ReceivedBy);
    }
}