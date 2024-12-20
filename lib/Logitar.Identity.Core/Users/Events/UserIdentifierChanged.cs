using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a custom identifier is added to an user, or updated.
/// </summary>
public record UserIdentifierChanged : DomainEvent, INotification
{
  /// <summary>
  /// Gets the key of the custom identifier.
  /// </summary>
  public Identifier Key { get; }
  /// <summary>
  /// Gets the value of the custom identifier.
  /// </summary>
  public CustomIdentifier Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIdentifierChanged"/> class.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="value">The value of the custom identifier.</param>
  public UserIdentifierChanged(Identifier key, CustomIdentifier value)
  {
    Key = key;
    Value = value;
  }
}
