using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Queries;

public record GetStudentDetailsResult(
    int Id,
    string FullName,
    DateTime DateOfBirth)
{
    public IEnumerable<string> Contacts { get; set; } = [];
    public IEnumerable<string> Errors { get; set; } = [];
};

public record GetStudentDetailsQuery() : IQuery<GetStudentDetailsResult>;

public sealed class GetStudentDetailsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetStudentDetailsQuery, GetStudentDetailsResult>
{
    public async Task<GetStudentDetailsResult> Handle(GetStudentDetailsQuery query, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetStudentDetailsResult>>)(() => HandleQueryAsync(query, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new GetStudentDetailsResult(0, string.Empty, default)
            {
                Errors = [$"An error occurred during registration: {ex.Message}"]
            });
    }
    
    private async Task<GetStudentDetailsResult> HandleQueryAsync(GetStudentDetailsQuery query, CancellationToken cancellationToken)
    {
        var student = await dbContext
            .Students
            .Include(s => s.ParentPhones)
            .Where(s => s.Id == 1).FirstOrDefaultAsync(cancellationToken);

        if (student is null)
        {
            return new GetStudentDetailsResult(0, string.Empty, default)
            {
                Errors = ["Student not found."]
            };
        }
        
        return new GetStudentDetailsResult(
            student.Id,
            student.ToString(),
            student.DateOfBirth)
        {
            Contacts = student.ParentPhones.Select(p => $"{p.Contact.CountryCode}{p.Contact.PhoneNumber}").ToList()
        };
    }
}