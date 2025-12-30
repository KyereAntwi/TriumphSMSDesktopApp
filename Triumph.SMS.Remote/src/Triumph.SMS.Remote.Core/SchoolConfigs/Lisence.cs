using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.Exceptions;

namespace Triumph.SMS.Remote.Core.SchoolConfigs;

public class Lisence : EntityBase<int>
{
    // for EF
    private Lisence()
    {
        
    }
    public string LisenceKey { get; private set; } = string.Empty;
    public DateTime IssuedOn { get; private set; }
    public DateTime ExpiresOn { get; private set; }
    public int SchoolId { get; private set; }
    public SchoolInformation? School { get; private set; }

    private Lisence(string lisenceKey, DateTime issuedOn, DateTime expiresOn, int schoolId)
    {
        LisenceKey = lisenceKey;
        IssuedOn = issuedOn;
        ExpiresOn = expiresOn;
        SchoolId = schoolId;
    }

    public static Lisence Create(string lisenceKey, int schoolId, DateTime issuedOn, DateTime expiresOn)
    {
        if (string.IsNullOrEmpty(lisenceKey) || lisenceKey.Count() < 12)
        {
            throw new BadValuesException("Key should not be empty and not less than 12 characters");
        }

        if(issuedOn.Date < DateTime.UtcNow.Date)
        {
            throw new BadValuesException("Issued date is not valid");
        }

        if(expiresOn.Date <= DateTime.UtcNow.Date)
        {
            throw new BadValuesException("Invalid expires date");
        }

        return new Lisence(lisenceKey, issuedOn, expiresOn, schoolId);
    }
}
