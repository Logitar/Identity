using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a session is renewed.
/// </summary>
public record SessionRenewed : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new secret of the session.
  /// </summary>
  public Password Secret { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionRenewed"/> class.
  /// </summary>
  /// <param name="secret">The new secret of the session.</param>
  public SessionRenewed(Password secret)
  {
    Secret = secret;
  }
}
