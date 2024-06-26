﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a role is added to an user.
/// </summary>
public class UserRoleAddedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserRoleAddedEvent"/> class.
  /// </summary>
  /// <param name="roleId">The role identifier.</param>
  public UserRoleAddedEvent(RoleId roleId)
  {
    RoleId = roleId;
  }
}
