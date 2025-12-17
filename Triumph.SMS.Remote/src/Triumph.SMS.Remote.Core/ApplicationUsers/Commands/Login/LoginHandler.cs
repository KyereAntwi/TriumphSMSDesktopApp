using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.ApplicationUsers.Enums;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.ApplicationUsers.Commands.Login;

public record LoginCommand(string Username, string Password) : ICommand<LoginResult>;

public record LoginResult(IEnumerable<Role>? Roles, IEnumerable<string>? Errors);

public sealed class LoginHandler(IApplicationDbContext dbContext) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        return await 
            ((Func<Task<LoginResult>>)(() => HandleLoginAsync(command, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new LoginResult(null, [$"An error occurred during login: {ex.Message}"]));
    }
    
    private async Task<LoginResult> HandleLoginAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext
            .ApplicationUsers
            .Where(u => u.Username == command.Username)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null || !user.VerifyPassword(command.Password))
        {
            return new LoginResult(null, ["Invalid username or password."]);
        }

        return new LoginResult(user.GetRoles(), []);
    }
}