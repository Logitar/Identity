using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// An abstraction of user password modifying events.
/// </summary>
public abstract record UserPasswordEvent : DomainEvent
{
  /// <summary>
  /// Gets the new password of the user.
  /// </summary>
  public Password Password { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier, generally the user's identifier.</param>
  /// <param name="password">The new password of the user.</param>
  protected UserPasswordEvent(ActorId actorId, Password password)
  {
    ActorId = actorId;
    Password = password;
  }
}
