using MediatR;

namespace Triumph.SMS.Remote.Core.Students.Events;

public record StudentRetiredEvent(string Student, ParentPhone ParentPhone) : INotification;