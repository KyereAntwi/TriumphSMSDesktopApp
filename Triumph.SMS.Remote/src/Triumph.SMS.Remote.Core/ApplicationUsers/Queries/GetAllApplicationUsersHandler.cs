using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.ApplicationUsers.Queries;

public record ApplicationUserDto(
    int Id, 
    string UserName, 
    string Email,
    string FullName);

public record GetAllApplicationUsersResult(
    IEnumerable<ApplicationUserDto> Users,
    int TotalCount)
{
    public IEnumerable<string> Errors { get; set; } = [];
};

public record GetAllApplicationUsersQuery(
    int Page = 1,
    int PageSize = 50,
    string? SearchTerm = null) : IQuery<GetAllApplicationUsersResult>;

public sealed class GetAllApplicationUsersHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<GetAllApplicationUsersQuery, GetAllApplicationUsersResult>
{
    public async Task<GetAllApplicationUsersResult> Handle(GetAllApplicationUsersQuery query, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetAllApplicationUsersResult>>)(() => HandleQueryAsync(query, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new GetAllApplicationUsersResult([], 0, [$"An error occurred during registration: {ex.Message}"]));
    }

    private async Task<GetAllApplicationUsersResult> HandleQueryAsync(GetAllApplicationUsersQuery query, CancellationToken cancellationToken)
    {
        var usersQuery = dbContext.ApplicationUsers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.ToLower();
            
            usersQuery = usersQuery.Where(u => 
                u.Username.ToLower().Contains(searchTerm) || 
                (u.Email != null && u.Email.ToLower().Contains(searchTerm)) ||
                (u.FirstName.ToLower().Contains(searchTerm)) ||
                (u.LastName.ToLower().Contains(searchTerm)));
        }

        var totalCount = await usersQuery.CountAsync(cancellationToken);

        var users = await usersQuery
            .OrderBy(u => u.LastName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(u => new ApplicationUserDto(
                u.Id,
                u.Username,
                u.Email ?? string.Empty,
                u.ToString()))
            .ToListAsync(cancellationToken);

        return new GetAllApplicationUsersResult(users, totalCount, []);
    }
}