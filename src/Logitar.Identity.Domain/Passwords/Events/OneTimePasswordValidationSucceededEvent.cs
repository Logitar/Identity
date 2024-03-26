using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) is successfully validated.
/// </summary>
public record OneTimePasswordValidationSucceededEvent : DomainEvent, INotification;
