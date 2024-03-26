using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a custom identifier is removed from an user.
/// </summary>
public record UserIdentifierRemovedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the key of the custom identifier.
  /// </summary>
  public string Key { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIdentifierRemovedEvent"/> class.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  public UserIdentifierRemovedEvent(string key)
  {
    Key = key;
  }
}
