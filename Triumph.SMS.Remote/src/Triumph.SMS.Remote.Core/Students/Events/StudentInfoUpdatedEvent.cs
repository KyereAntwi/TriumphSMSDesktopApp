using MediatR;

namespace Triumph.SMS.Remote.Core.Students.Events;

public record StudentInfoUpdatedEvent(string FirstName, ParentPhone ParentPhone) : INotification;