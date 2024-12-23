using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// An abstraction of user password modifying events.
/// </summary>
public abstract record UserPasswordEvent : DomainEvent
{
  /// <summary>
  /// Gets the new password of the user.
  /// </summary>
  public Password Password { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserPasswordEvent"/> class.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  protected UserPasswordEvent(Password password)
  {
    Password = password;
  }
}
