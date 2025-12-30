using FluentValidation;
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
public record RegisterResult(bool Success)
{
    public IEnumerable<string>? Errors { get; init; } = [];
};

public sealed class RegisterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return await 
            ((Func<Task<RegisterResult>>)(() => HandleRegisterAsync(command, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new RegisterResult(false)
            {
                Errors = [$"An error occurred during registration: {ex.Message}"]
            });
    }

    private async Task<RegisterResult> HandleRegisterAsync(RegisterCommand command, CancellationToken cancellationToken)
    {
        var validation = new RegisterCommandValidator();
        var validationResult = await validation.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) 
        { 
            return new RegisterResult(false)
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };
        }

        var existingUser = await dbContext
            .ApplicationUsers
            .Where(u => u.Username == command.Username)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
        {
            return new RegisterResult(false) { Errors = ["Username is already taken."] };
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

        return new RegisterResult(true);
    }
}

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters long.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("A valid email address is required.");
        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("At least one role must be assigned to the user.");
        RuleFor(x => x.OtherNames)
            .MaximumLength(100).WithMessage("Other names cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.OtherNames));
    }
}