using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.ApplicationUsers.Enums;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.ApplicationUsers.Commands.Register;

public record RegisterCommand(
    string Username, 
    string Password,
    string FirstName,
    string LastName,
    string? Email,
    string? OtherNames,
    IEnumerable<Role>? Roles,
    IEnumerable<PrimaryPhone>? Phones) : ICommand<RegisterResult>;
public record RegisterResult(bool Success, IEnumerable<string>? Errors);

public sealed class RegisterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return await 
            ((Func<Task<RegisterResult>>)(() => HandleRegisterAsync(command, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new RegisterResult(false, [$"An error occurred during registration: {ex.Message}"]));
    }

    private async Task<RegisterResult> HandleRegisterAsync(RegisterCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext
            .ApplicationUsers
            .Where(u => u.Username == command.Username)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
        {
            return new RegisterResult(false, ["Username is already taken."]);
        }

        var newUser = ApplicationUser.Register(
            command.FirstName,
            command.LastName,
            command.OtherNames ?? string.Empty,
            command.Username,
            command.Email,
            command.Password,
            command.Roles,
            command.Phones);

        await dbContext.ApplicationUsers.AddAsync(newUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterResult(true, null);
    }
}