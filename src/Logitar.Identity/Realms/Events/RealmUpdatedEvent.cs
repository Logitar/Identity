using MediatR;

namespace Logitar.Identity.Realms.Events;

/// <summary>
/// The event raised when a <see cref="RealmAggregate"/> is updated.
/// </summary>
public record RealmUpdatedEvent : RealmSavedEvent, INotification;
