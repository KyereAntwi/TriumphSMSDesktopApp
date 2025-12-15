using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students;

public class Student : EntityBase<int>
{
    // For EF
    private Student()
    {
    }
    
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? Residence { get; set; }
    public DateTime DateOfBirth { get; private set; }
    public Contact? PrimaryContact { get; private set; }
    
    public Student Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string? residence = null,
        Contact? primaryContact = null)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PrimaryContact = primaryContact;
        Residence = residence ?? string.Empty;
        return this;
    }
    
    
}
