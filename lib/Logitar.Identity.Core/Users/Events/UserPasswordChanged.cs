using Logitar.Identity.Core.Passwords;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a user changes its password.
/// </summary>
public record UserPasswordChanged : UserPasswordEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordChanged"/> class.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  public UserPasswordChanged(Password password) : base(password)
  {
  }
}
