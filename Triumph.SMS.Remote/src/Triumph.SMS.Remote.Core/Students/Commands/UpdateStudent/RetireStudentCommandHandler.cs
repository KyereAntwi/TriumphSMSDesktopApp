using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Commands.UpdateStudent;

public sealed class RetireStudentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RetireStudentCommand, UpdateStudentInfoCommandResult>
{
    public async Task<UpdateStudentInfoCommandResult> Handle(RetireStudentCommand request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<UpdateStudentInfoCommandResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new UpdateStudentInfoCommandResult
            {
                Errors = [$"There was a problem when retiring student. Error = ${ex.Message}"]
            });
    }

    private async Task<UpdateStudentInfoCommandResult> HandleAsync(RetireStudentCommand command, CancellationToken cancellationToken)
    {
        var student = await dbContext
            .Students
            .AsTracking()
            .Where(s => s.Id == command.StudentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (student == null)
        {
            return new UpdateStudentInfoCommandResult
            {
                Errors = [$"Student with Id {command.StudentId} was not found."]
            };
        }

        student.RetireStudent();
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateStudentInfoCommandResult();
    }
}
