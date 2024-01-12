using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when the password of an user is modified.
/// </summary>
public record UserPasswordUpdatedEvent : UserPasswordEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordUpdatedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="password">The new password of the user.</param>
  public UserPasswordUpdatedEvent(ActorId actorId, Password password) : base(actorId, password)
  {
  }
}
