using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when the phone number of an user is updated.
/// </summary>
public record UserPhoneChangedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new phone number of the user.
  /// </summary>
  public PhoneUnit? Phone { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAddressChangedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="phone">The new phone number of the user.</param>
  public UserPhoneChangedEvent(ActorId actorId, PhoneUnit? phone)
  {
    ActorId = actorId;
    Phone = phone;
  }
}
