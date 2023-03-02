using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when a new <see cref="UserAggregate"/> is updated.
/// </summary>
public record UserUpdatedEvent : UserSavedEvent, INotification;
