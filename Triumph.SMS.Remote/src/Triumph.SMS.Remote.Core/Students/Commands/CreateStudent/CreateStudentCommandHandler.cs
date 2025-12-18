using MediatR;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.Students.Commands.CreateStudent;

public sealed class CreateStudentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateStudentCommand, CreateStudentCommandResult>
{
    public async Task<CreateStudentCommandResult> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
    {
        return await ((Func<Task<CreateStudentCommandResult>>)(() => HandleAsync(command, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new CreateStudentCommandResult(0) 
            {
                Errors = [$"There was a problem when creating student. Error = ${ex.Message}"]
            });
    }

    private async Task<CreateStudentCommandResult> HandleAsync(CreateStudentCommand command, CancellationToken cancellationToken)
    {
        var newStudent = new Student();

        newStudent = newStudent.Create(
            command.FirstName,
            command.LastName,
            command.DateOfBirth,
            ParentPhone.Create(command.ParentContact, 0, command.ParentContactType),
            command.OtherNames,
            command.Residence,
            false);

        await dbContext.Students.AddAsync(newStudent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateStudentCommandResult(newStudent.Id)
        {
            Errors = Array.Empty<string>()
        };
    }
}
