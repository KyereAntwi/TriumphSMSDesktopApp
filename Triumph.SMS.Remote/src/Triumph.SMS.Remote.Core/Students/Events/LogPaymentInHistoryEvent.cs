using MediatR;

namespace Triumph.SMS.Remote.Core.Students.Events;

public record LogPaymentInHistoryEvent(RecentPayment Payment) : INotification;