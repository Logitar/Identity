using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a session is deleted.
/// </summary>
public record SessionDeleted : DomainEvent, IDeleteEvent, INotification;
