using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Commands.UpdateStudent;

public sealed class UpdateStudentInfoCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateStudentInfoCommand, UpdateStudentInfoCommandResult>
{
    public async Task<UpdateStudentInfoCommandResult> Handle(UpdateStudentInfoCommand request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<UpdateStudentInfoCommandResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new UpdateStudentInfoCommandResult
            {
                Errors = [$"There was a problem when updating student info. Error = ${ex.Message}"]
            });
    }

    private async Task<UpdateStudentInfoCommandResult> HandleAsync(UpdateStudentInfoCommand command, CancellationToken cancellationToken)
    {
        var student = await dbContext
            .Students
            .AsTracking()
            .Where(s => s.Id == command.StudentId)
            .FirstOrDefaultAsync(cancellationToken);

        if(student is null)
        {
            return new UpdateStudentInfoCommandResult
            {
                Errors = [$"Student with Id {command.StudentId} was not found."]
            };
        }

        // TODO - check for notification feature enabled from settings

        student.UpdateInfo(
            command.FirstName,
            command.LastName,
            command.DateOfBirth,
            command.Residence,
            command.OtherNames);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateStudentInfoCommandResult();
    }
}
