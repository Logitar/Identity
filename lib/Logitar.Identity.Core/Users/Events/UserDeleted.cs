using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a user is deleted.
/// </summary>
public record UserDeleted : DomainEvent, IDeleteEvent, INotification;
