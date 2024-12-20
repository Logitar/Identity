using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a custom identifier is removed from an user.
/// </summary>
public record UserIdentifierRemoved : DomainEvent, INotification
{
  /// <summary>
  /// Gets the key of the custom identifier.
  /// </summary>
  public Identifier Key { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIdentifierRemoved"/> class.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  public UserIdentifierRemoved(Identifier key)
  {
    Key = key;
  }
}
