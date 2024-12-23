using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Passwords.Events;

/// <summary>
/// The event raised when a One-Time Password (OTP) is deleted.
/// </summary>
public record OneTimePasswordDeleted : DomainEvent, IDeleteEvent, INotification;
