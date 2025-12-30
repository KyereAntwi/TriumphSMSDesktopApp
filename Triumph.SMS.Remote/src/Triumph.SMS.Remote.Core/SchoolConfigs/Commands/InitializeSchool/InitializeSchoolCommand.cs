using FluentValidation;
using Triumph.SMS.Remote.Core.Common.CQRS;

namespace Triumph.SMS.Remote.Core.SchoolConfigs.Commands.InitializeSchool;

public record InitializeSchoolCommandResult
{
    public IEnumerable<string> Errors { get; set; } = [];
}

public record InitializeSchoolCommand : ICommand<InitializeSchoolCommandResult>
{
    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? Email { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Motto { get; set; }
}

public class InitializeSchoolCommandValidator: AbstractValidator<InitializeSchoolCommand>
{
    public InitializeSchoolCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("School name is required.")
            .MaximumLength(300).WithMessage("School name must not exceed 300 characters.");

        RuleFor(x => x.Logo)
            .MaximumLength(500).WithMessage("Logo URL must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Logo));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(225).WithMessage("Email must not exceed 225 characters.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MinimumLength(10).WithMessage("Phone number must be at least 10 characters long.")
            .MaximumLength(15).WithMessage("Phone number must not exceed 15 characters.");

        RuleFor(x => x.Website)
            .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$").WithMessage("Invalid website URL format.")
            .MaximumLength(225).WithMessage("Website URL must not exceed 225 characters.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(x => x.Motto)
            .MaximumLength(300).WithMessage("Motto must not exceed 300 characters.")
            .When(x => !string.IsNullOrEmpty(x.Motto));
    }
}
