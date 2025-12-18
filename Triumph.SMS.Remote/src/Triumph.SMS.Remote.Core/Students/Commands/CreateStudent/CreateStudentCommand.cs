using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Enums;
using Triumph.SMS.Remote.Core.Common.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students.Commands.CreateStudent;

public record CreateStudentCommand : ICommand<CreateStudentCommandResult>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? OtherNames { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Contact ParentContact { get; set; } = null!;
    public string? Residence { get; set; }
    public ContactType ParentContactType { get; set; } = ContactType.Primary;
}

public record CreateStudentCommandResult(int StudentId)
{
    public IEnumerable<string> Errors { get; set; } = [];
};
