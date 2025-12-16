namespace Triumph.SMS.Remote.Core.Common.Entities;

public abstract class Person : EntityBase<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? OtherNames { get; set; }
    public DateTime DateOfBirth { get; set; }


    override public string ToString()
    {
        return $"{FirstName} {OtherNames ?? string.Empty} {LastName}";
    }

    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}
