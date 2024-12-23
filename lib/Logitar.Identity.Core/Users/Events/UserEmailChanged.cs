using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the email address of a user is updated.
/// </summary>
public record UserEmailChanged : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new email address of the user.
  /// </summary>
  public Email? Email { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserEmailChanged"/> class.
  /// </summary>
  /// <param name="email">The new email address of the user.</param>
  public UserEmailChanged(Email? email)
  {
    Email = email;
  }
}
