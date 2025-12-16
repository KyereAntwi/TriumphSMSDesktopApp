using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.Enums;
using Triumph.SMS.Remote.Core.Common.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students;

public class ParentPhone : EntityBase<int>
{
    // For EF
    private ParentPhone()
    {
    }
    
    private ParentPhone(Contact contact, int studentId, ContactType type)
    {
        Contact = contact;
        StudentId = studentId;
        Type = type;
    }
    
    public static ParentPhone Create(Contact contact, int studentId, ContactType type = ContactType.Primary)
    {
        var parentPhone = new ParentPhone(contact, studentId, type);
        return parentPhone;
    }

    public Contact Contact { get; private set; }
    public ContactType Type { get; private set; } = ContactType.Primary;
    
    public int StudentId { get; private set; }
    public Student? Student { get; set; }
}