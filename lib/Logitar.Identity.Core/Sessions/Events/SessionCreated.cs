using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Users;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a new session is created.
/// </summary>
public record SessionCreated : DomainEvent, INotification
{
  /// <summary>
  /// Gets the identifier of the user owning the session.
  /// </summary>
  public UserId UserId { get; }
  /// <summary>
  /// Gets the secret of the session.
  /// </summary>
  public Password? Secret { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionCreated"/> class.
  /// </summary>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="userId">The identifier of the user owning the session.</param>
  public SessionCreated(Password? secret, UserId userId)
  {
    Secret = secret;
    UserId = userId;
  }
}
