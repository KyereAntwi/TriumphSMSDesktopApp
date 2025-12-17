using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.ApplicationUsers.Enums;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.ApplicationUsers.Queries;

public record GetApplicationUserDetailsResult(
    int Id,
    string UserName,
    string Email,
    DateTime DateOfBirth,
    string FullName,
    DateTime CreatedAt)
{
    public IEnumerable<string> Phones { get; set; } = [];
    public IEnumerable<Role> Roles { get; set; } = [];
    public IEnumerable<string> Errors { get; set; } = [];
}

public record GetApplicationUserDetailsQuery(int Id) : IQuery<GetApplicationUserDetailsResult>;

public sealed class GetApplicationUserDetailsQueryHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<GetApplicationUserDetailsQuery, GetApplicationUserDetailsResult>
{
    public async Task<GetApplicationUserDetailsResult> Handle(GetApplicationUserDetailsQuery request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetApplicationUserDetailsResult>>)(() => HandleQueryAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => 
                new GetApplicationUserDetailsResult(
                    0, string.Empty, string.Empty, DateTime.MinValue, string.Empty, DateTime.MinValue)
            {
                Errors = [$"An error occurred while retrieving user details: {ex.Message}"]
            });
    }

    private async Task<GetApplicationUserDetailsResult> HandleQueryAsync(GetApplicationUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext
            .ApplicationUsers
            .Include(u => u.Contacts)
            .AsSplitQuery()
            .Where(u => u.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            return new GetApplicationUserDetailsResult(0, string.Empty, string.Empty, DateTime.MinValue, string.Empty, DateTime.MinValue)
            {
                Errors = [$"User with ID {request.Id} not found."]
            };
        }

        return new GetApplicationUserDetailsResult(
            user.Id,
            user.Username,
            user.Email ?? string.Empty,
            user.DateOfBirth,
            user.ToString(),
            user.CreatedAt)
        {
            Phones = user.Contacts.Select(c => c.ToString())!,
            Roles = user.GetRoles()
        };
    }
}