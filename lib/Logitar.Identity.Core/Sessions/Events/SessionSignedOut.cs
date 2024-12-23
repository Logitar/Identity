using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when an active session is signed-out.
/// </summary>
public record SessionSignedOut : DomainEvent, INotification;
