using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when a role is deleted.
/// </summary>
public record RoleDeleted : DomainEvent, IDeleteEvent, INotification;
