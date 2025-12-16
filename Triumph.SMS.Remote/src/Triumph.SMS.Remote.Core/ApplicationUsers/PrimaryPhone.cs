using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.ValueObjects;

namespace Triumph.SMS.Remote.Core.ApplicationUsers;

public class PrimaryPhone : EntityBase<int>
{
    // For EF
    private PrimaryPhone() {}

    public Contact Contact { get; private set; }
    public int ApplicationUserId { get; private set; }
    public ApplicationUser? ApplicationUser { get; private set; } = null;
}
