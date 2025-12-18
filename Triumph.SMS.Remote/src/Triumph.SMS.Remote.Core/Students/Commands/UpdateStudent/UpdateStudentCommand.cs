using Triumph.SMS.Remote.Core.Common.CQRS;

namespace Triumph.SMS.Remote.Core.Students.Commands.UpdateStudent;

public record UpdateStudentInfoCommandResult
{
    public IEnumerable<string> Errors { get; set; } = [];
}

public record UpdateStudentInfoCommand(
    int StudentId,
    string FirstName,
    string LastName,
    string? OtherNames,
    DateTime DateOfBirth,
    string Residence) : ICommand<UpdateStudentInfoCommandResult>;

public record RetireStudentCommand(
    int StudentId) : ICommand<UpdateStudentInfoCommandResult>;
