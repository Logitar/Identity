using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user resets its password.
/// </summary>
public record UserPasswordResetEvent : UserPasswordEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordResetEvent"/> class.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  public UserPasswordResetEvent(Password password) : base(password)
  {
  }
}
