using MediatR;
using Triumph.SMS.Remote.Core.Payments.ValueObjects;

namespace Triumph.SMS.Remote.Core.Students.Events;

public record StudentPaymentMade(
    string Student,
    Money AmountPaid,
    ParentPhone ParentPhone) : INotification;