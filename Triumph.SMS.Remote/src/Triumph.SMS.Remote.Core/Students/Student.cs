using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students;

public class Student : EntityBase<int>
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public Contact? PrimaryContact { get; private set; }
}
