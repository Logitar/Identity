using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when an <see cref="UserAggregate"/> is updated.
/// </summary>
public record UserUpdatedEvent : UserSavedEvent, INotification;
