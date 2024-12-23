using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the postal address of a user is updated.
/// </summary>
public record UserAddressChanged : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new postal address of the user.
  /// </summary>
  public Address? Address { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAddressChanged"/> class.
  /// </summary>
  /// <param name="address">The new postal address of the user.</param>
  public UserAddressChanged(Address? address)
  {
    Address = address;
  }
}
