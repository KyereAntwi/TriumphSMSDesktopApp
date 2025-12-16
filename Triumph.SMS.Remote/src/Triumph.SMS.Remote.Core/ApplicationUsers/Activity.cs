using Triumph.SMS.Remote.Core.Common.Entities;

namespace Triumph.SMS.Remote.Core.ApplicationUsers;

public class Activity : EntityBase<int>
{
    // For EF
    private Activity() {}

    private Activity(int applicationUserId, string description)
    {
        ApplicationUserId = applicationUserId;
        Description = description;
    }

    public static Activity Create(int applicationUserId, string description)
    {
        return new Activity(applicationUserId, description);
    }

    public int ApplicationUserId { get; private set; }
    public ApplicationUser? ApplicationUser { get; private set; } = null;

    public string Description { get; private set; } = string.Empty;
}
