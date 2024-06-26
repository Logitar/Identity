﻿using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

/// <summary>
/// The event raised when a role is deleted.
/// </summary>
public class RoleDeletedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleDeletedEvent"/> class.
  /// </summary>
  public RoleDeletedEvent()
  {
    IsDeleted = true;
  }
}
