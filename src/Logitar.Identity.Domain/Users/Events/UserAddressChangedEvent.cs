﻿using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when the postal address of an user is updated.
/// </summary>
public class UserAddressChangedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new postal address of the user.
  /// </summary>
  public AddressUnit? Address { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAddressChangedEvent"/> class.
  /// </summary>
  /// <param name="address">The new postal address of the user.</param>
  public UserAddressChangedEvent(AddressUnit? address)
  {
    Address = address;
  }
}
