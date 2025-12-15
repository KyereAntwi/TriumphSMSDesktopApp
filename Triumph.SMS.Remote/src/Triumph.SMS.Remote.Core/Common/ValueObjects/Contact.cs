
using Triumph.SMS.Remote.Core.Common.Exceptions;

namespace Triumph.SMS.Remote.Core.Common.ValueObjects;

public class Contact : ValueObjectBase
{
    public string CountryCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // For EF
    private Contact() { }

    private Contact(string countryCode, string phoneNumber)
    {
        CountryCode = countryCode;
        PhoneNumber = phoneNumber;
    }

    public static Contact Create(string countryCode, string phoneNumber)
    {
        if (CodeIsNotValid(countryCode) || PhoneNumberIsNotValid(phoneNumber))
        {
            throw new EntityValidationException("Invalid contact information.");
        }
        
        return new Contact(countryCode, phoneNumber);
    }

    private static bool PhoneNumberIsNotValid(string phoneNumber)
    {
        return phoneNumber.Length is < 9 or > 15;
    }

    private static bool CodeIsNotValid(string countryCode)
    {
        return countryCode.Length < 1 || countryCode.Length > 4 || !countryCode.StartsWith("+");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return PhoneNumber;
    }
}
