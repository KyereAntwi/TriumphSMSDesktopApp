using MediatR;

namespace Triumph.SMS.Remote.Core.Students.Events;

public record StudentCreatedEvent(Student NewStudent) : INotification;