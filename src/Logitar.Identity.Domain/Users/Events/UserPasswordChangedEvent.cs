﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user changes its password.
/// </summary>
public record UserPasswordChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets the new password of the user.
  /// </summary>
  public Password Password { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordChangedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier, generally the user's identifier.</param>
  /// <param name="password">The new password of the user.</param>
  public UserPasswordChangedEvent(ActorId actorId, Password password)
  {
    ActorId = actorId;
    Password = password;
  }
}
