﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

/// <summary>
/// The event raised when a new session is created.
/// </summary>
public class SessionCreatedEvent : DomainEvent, INotification
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
  /// Initializes a new instance of the <see cref="SessionCreatedEvent"/> class.
  /// </summary>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="userId">The identifier of the user owning the session.</param>
  public SessionCreatedEvent(Password? secret, UserId userId)
  {
    Secret = secret;
    UserId = userId;
  }
}
