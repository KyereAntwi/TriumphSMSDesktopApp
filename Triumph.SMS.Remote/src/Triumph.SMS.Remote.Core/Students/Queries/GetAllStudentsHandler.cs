using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Queries;

public record StudentDto(
    int Id,
    string FullName);
public record GetAllStudentsQueryResult(
    IEnumerable<StudentDto> Students, 
    int TotalCount, 
    IEnumerable<string> Errors);

public record GetAllStudentsQuery(
    string SearchTerm = "",
    DateTime CreatedFromDate = default,
    DateTime CreatedToDate = default,
    int Page = 1,
    int PageSize = 50) 
    : IQuery<GetAllStudentsQueryResult>;

public sealed class GetAllStudentsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAllStudentsQuery, GetAllStudentsQueryResult>
{
    public async Task<GetAllStudentsQueryResult> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetAllStudentsQueryResult>>)(() => HandleQueryAsync(query, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new GetAllStudentsQueryResult([], 0, [$"An error occurred during registration: {ex.Message}"]));
    }

    private async Task<GetAllStudentsQueryResult> HandleQueryAsync(GetAllStudentsQuery query, CancellationToken cancellationToken)
    {
        var studentsQuery = dbContext.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            studentsQuery = studentsQuery.Where(s => s.ToString().ToLower().Contains(query.SearchTerm.ToLower()));
        }

        if (query.CreatedFromDate != default)
        {
            studentsQuery = studentsQuery.Where(s => s.CreatedAt >= query.CreatedFromDate);
        }

        if (query.CreatedToDate != default)
        {
            studentsQuery = studentsQuery.Where(s => s.CreatedAt <= query.CreatedToDate);
        }

        var skip = (query.Page - 1) * query.PageSize;

        var students = await studentsQuery
            .Skip(skip)
            .Take(query.PageSize)
            .Select(s => new StudentDto(
                s.Id,
                s.ToString()))
            .ToListAsync(cancellationToken);
        
        var totalCount = await studentsQuery.CountAsync(cancellationToken);
        
        return new GetAllStudentsQueryResult(
            Students: students,
            TotalCount: totalCount,
            Errors: []);
    }
}