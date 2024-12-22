using Logitar.Identity.Core.Passwords;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a user resets its password.
/// </summary>
public record UserPasswordReset : UserPasswordEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordReset"/> class.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  public UserPasswordReset(Password password) : base(password)
  {
  }
}
