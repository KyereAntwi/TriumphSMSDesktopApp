
namespace Triumph.SMS.Remote.Core.Common.ValueObjects;

public class Contact : ValueObjectBase
{
    public string CountryCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    private Contact() { }

    private Contact(string countryCode, string phoneNumber)
    {
        CountryCode = countryCode;
        PhoneNumber = phoneNumber;
    }

    public static Contact Create(string countryCode, string phoneNumber)
    {
        // Potentially add validation logic here
        return new Contact(countryCode, phoneNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return PhoneNumber;
    }
}
