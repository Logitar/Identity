using MediatR;

namespace Logitar.Identity.Roles.Events;

/// <summary>
/// Represents the event raised when a <see cref="RoleAggregate"/> is updated.
/// </summary>
public record RoleUpdatedEvent : RoleSavedEvent, INotification
{
}
