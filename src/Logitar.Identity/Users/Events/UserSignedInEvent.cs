using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when an <see cref="UserAggregate"/> signs-in.
/// </summary>
public record UserSignedInEvent : DomainEvent, INotification;
