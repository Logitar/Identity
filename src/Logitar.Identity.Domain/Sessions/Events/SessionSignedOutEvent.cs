using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when an active session is signed-out.
/// </summary>
public class SessionSignedOutEvent : DomainEvent, INotification;
