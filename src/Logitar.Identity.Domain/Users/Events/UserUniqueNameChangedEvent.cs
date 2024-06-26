﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when the unique name of an user is changed.
/// </summary>
public class UserUniqueNameChangedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  public UniqueNameUnit UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserUniqueNameChangedEvent"/> class.
  /// </summary>
  /// <param name="uniqueName">The unique name of the user.</param>
  public UserUniqueNameChangedEvent(UniqueNameUnit uniqueName)
  {
    UniqueName = uniqueName;
  }
}
