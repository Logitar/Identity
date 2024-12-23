using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the phone number of a user is updated.
/// </summary>
public record UserPhoneChanged : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new phone number of the user.
  /// </summary>
  public Phone? Phone { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserPhoneChanged"/> class.
  /// </summary>
  /// <param name="phone">The new phone number of the user.</param>
  public UserPhoneChanged(Phone? phone)
  {
    Phone = phone;
  }
}
