using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a custom identifier is added to an user, or updated.
/// </summary>
public record UserIdentifierChangedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the key of the custom identifier.
  /// </summary>
  public string Key { get; }
  /// <summary>
  /// Gets the value of the custom identifier.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIdentifierChangedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="value">The value of the custom identifier.</param>
  public UserIdentifierChangedEvent(ActorId actorId, string key, string value)
  {
    ActorId = actorId;
    Key = key;
    Value = value;
  }
}
