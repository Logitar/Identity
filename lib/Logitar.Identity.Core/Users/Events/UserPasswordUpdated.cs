using Logitar.Identity.Core.Passwords;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the password of an user is modified.
/// </summary>
public record UserPasswordUpdated : UserPasswordEvent, INotification
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordUpdated"/> class.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  public UserPasswordUpdated(Password password) : base(password)
  {
  }
}
