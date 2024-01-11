using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when a session is renewed.
/// </summary>
public record SessionRenewedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new secret of the session.
  /// </summary>
  public Password Secret { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionRenewedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="secret">The new secret of the session.</param>
  public SessionRenewedEvent(ActorId actorId, Password secret)
  {
    ActorId = actorId;
    Secret = secret;
  }
}
