using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.Exceptions;
using Triumph.SMS.Remote.Core.Students.Events;

namespace Triumph.SMS.Remote.Core.Students;

public class Student : EntityBase<int>
{
    // For EF
    private Student()
    {
    }

    private ICollection<ParentPhone> _ParentPhones { get; set; } = new List<ParentPhone>();
    public IReadOnlyCollection<ParentPhone> ParentPhones => _ParentPhones.ToList().AsReadOnly();
    
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? OtherNames { get; set; }
    public string? Residence { get; set; }
    public DateTime DateOfBirth { get; private set; }
    public RecentPayment? RecentPayment { get; private set; }
    
    public Student Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        ParentPhone parentPhone,
        string? otherNames = null,
        string? residence = null)
    {
        if (dateOfBirth < DateTime.UtcNow.AddYears(-120) || dateOfBirth > DateTime.UtcNow)
        {
            throw new EntityValidationException("Date of birth is not valid.");
        }
        
        var newStudent = new Student
        {
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            Residence = residence ?? string.Empty,
            OtherNames = otherNames ?? string.Empty
        };
        
        newStudent._ParentPhones.Add(parentPhone);
        
        AddDomainEvent(new StudentCreatedEvent(newStudent));
        
        return newStudent;
    }
    public void UpdateInfo(
        string firstName, 
        string lastName, 
        DateTime dateOfBirth, 
        string? residence = null, 
        string? otherNames = null)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Residence = residence ?? Residence;
        OtherNames = otherNames ?? OtherNames;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new StudentInfoUpdatedEvent(FirstName, ParentPhones.First()));
    }
    public void AddParentPhone(ParentPhone parentPhone)
    {
        _ParentPhones.Add(parentPhone);
        UpdatedAt = DateTime.UtcNow;
    }
    public void RemoveParentPhone(ParentPhone parentPhone)
    {
        _ParentPhones.Remove(parentPhone);
        UpdatedAt = DateTime.UtcNow;
    }
    public string MakePayment(RecentPayment recentPayment)
    {
        // Send domain event to record current payment in payment history
        if (RecentPayment != null)
        {
            AddDomainEvent(new LogPaymentInHistoryEvent(RecentPayment));
        }
        
        RecentPayment = recentPayment;
        UpdatedAt = DateTime.UtcNow;
        
        // Send domain event to notify that a new payment has been made
        AddDomainEvent(new StudentPaymentMade(
            ToString(),
            recentPayment.Amount,
            ParentPhones.First()));
        
        return RecentPayment.Id.ToString();
    }
    public override string ToString()
    {
        return $"{FirstName} {OtherNames ?? string.Empty} {LastName}";
    }
}
