using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the event raised when an <see cref="UserAggregate"/>'s password is changed.
/// </summary>
public record UserChangedPasswordEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the new salted and hashed password of the user.
  /// </summary>
  public string PasswordHash { get; init; } = string.Empty;
}
