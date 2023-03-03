using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Events;

/// <summary>
/// Represents the event raised when a <see cref="SessionAggregate"/> is signed-out.
/// </summary>
public record SessionSignedOutEvent : DomainEvent, INotification;
