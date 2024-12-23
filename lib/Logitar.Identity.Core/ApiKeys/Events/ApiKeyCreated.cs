using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when a new API key is created.
/// </summary>
public record ApiKeyCreated : DomainEvent, INotification
{
  /// <summary>
  /// Gets the display name of the API key.
  /// </summary>
  public DisplayName DisplayName { get; }

  /// <summary>
  /// Gets the secret of the API key.
  /// </summary>
  public Password Secret { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyCreated"/> class.
  /// </summary>
  /// <param name="displayName">The display name of the API key.</param>
  /// <param name="secret">The secret of the API key.</param>
  public ApiKeyCreated(DisplayName displayName, Password secret)
  {
    DisplayName = displayName;
    Secret = secret;
  }
}
