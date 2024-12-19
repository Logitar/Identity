using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) validation failed.
/// </summary>
public record OneTimePasswordValidationFailed : DomainEvent, INotification;
