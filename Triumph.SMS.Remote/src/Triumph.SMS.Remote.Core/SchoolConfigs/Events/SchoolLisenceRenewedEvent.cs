using MediatR;

namespace Triumph.SMS.Remote.Core.SchoolConfigs.Events;

public record SchoolLisenceRenewedEvent(string LisenceKey, DateTime ExpiresOn) : INotification;