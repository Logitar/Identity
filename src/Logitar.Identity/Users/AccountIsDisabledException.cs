namespace Logitar.Identity.Users;

/// <summary>
/// The exception thrown when a disabled user try to sign-in to its account.
/// </summary>
public class AccountIsDisabledException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AccountIsDisabledException"/> class using the specified user.
  /// </summary>
  /// <param name="user">The user whose account is disabled.</param>
  public AccountIsDisabledException(UserAggregate user) : base($"The user account '{user}' is disabled.")
  {
    Data["User"] = user.ToString();
  }
}
