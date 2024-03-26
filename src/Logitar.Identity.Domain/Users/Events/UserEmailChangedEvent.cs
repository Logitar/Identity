using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when the email address of an user is updated.
/// </summary>
public record UserEmailChangedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new email address of the user.
  /// </summary>
  public EmailUnit? Email { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAddressChangedEvent"/> class.
  /// </summary>
  /// <param name="email">The new email address of the user.</param>
  public UserEmailChangedEvent(EmailUnit? email)
  {
    Email = email;
  }
}
