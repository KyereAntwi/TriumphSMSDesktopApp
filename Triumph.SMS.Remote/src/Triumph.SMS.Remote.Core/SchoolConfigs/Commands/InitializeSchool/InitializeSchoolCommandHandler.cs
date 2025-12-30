using MediatR;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.SchoolConfigs.Commands.InitializeSchool;

public sealed class InitializeSchoolCommandHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<InitializeSchoolCommand, InitializeSchoolCommandResult>
{
    public async Task<InitializeSchoolCommandResult> Handle(InitializeSchoolCommand request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<InitializeSchoolCommandResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new InitializeSchoolCommandResult
            {
                Errors = [$"An error occured during operation. Error: {ex.Message}"]
            });
    }

    private async Task<InitializeSchoolCommandResult> HandleAsync(InitializeSchoolCommand command, CancellationToken cancellationToken)
    {
        var validation = new InitializeSchoolCommandValidator();
        var validationResult = await validation.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new InitializeSchoolCommandResult
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };
        }

        var schoolInfo = SchoolInformation.Initialize(
            command.Name,
            command.Logo,
            command.Address,
            command.Phone,
            command.Email,
            command.Website,
            command.Motto);

        await dbContext.SchoolInformation.AddAsync(schoolInfo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new InitializeSchoolCommandResult();
    }
}
