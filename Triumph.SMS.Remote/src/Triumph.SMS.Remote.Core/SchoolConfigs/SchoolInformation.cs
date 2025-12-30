using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.SchoolConfigs.Events;

namespace Triumph.SMS.Remote.Core.SchoolConfigs;

public class SchoolInformation : EntityBase<int>
{
    // for EF
    private SchoolInformation()
    { }

    private ICollection<Lisence> _Lisences { get; set; } = [];
    public IReadOnlyCollection<Lisence> Lisences => _Lisences.ToList().AsReadOnly();

    public string Name { get; private set; } = string.Empty;
    public string? Logo { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Website { get; private set; }
    public string Motto { get; private set; } = string.Empty;

    private SchoolInformation(string name, string? logo, string address, string phone, string? email, string? website, string? motto)
    {
        Name = name;
        Logo = logo;
        Address = address;
        PhoneNumber = phone;
        Email = email;
        Website = website;
        Motto = motto ?? string.Empty;
    }

    public static SchoolInformation Initialize(string name, string? logo, string address, string phone, string? email, string? website, string? motto)
    {
        return new SchoolInformation(name, logo, address, phone, email, website, motto);
    }

    public void RenewLisence(Lisence lisence, bool messageNotificationFeatureEnabled = false)
    {
        if (_Lisences.Any(l => l.LisenceKey == lisence.LisenceKey))
        {
            throw new InvalidOperationException("A lisence with key already exist");
        }

        var existingActiveLisence = _Lisences.FirstOrDefault(l => l.ExpiresOn > DateTime.UtcNow);

        if (existingActiveLisence is not null)
        {
            var daysLeft = (existingActiveLisence.ExpiresOn - DateTime.UtcNow).Days;
            existingActiveLisence.ExpiresOn.AddDays(-1);

            _Lisences.Add(lisence);
        }
        else 
        {
            _Lisences.Add(lisence);
        }

        UpdatedAt = DateTime.UtcNow;

        if(messageNotificationFeatureEnabled)
            AddDomainEvent(new SchoolLisenceRenewedEvent(lisence.LisenceKey, lisence.ExpiresOn));
    }

    public bool HasActiveLisence()
    {
        return _Lisences.Any(l => l.ExpiresOn > DateTime.UtcNow);
    }
}
