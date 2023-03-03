namespace Logitar.Identity.Users;

/// <summary>
/// The exception thrown when an user tries signing-in to an account that is not confirmed.
/// </summary>
public class AccountIsNotConfirmedException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AccountIsNotConfirmedException"/> class using the specified user.
  /// </summary>
  /// <param name="user">The user whose account is not confirmed.</param>
  public AccountIsNotConfirmedException(UserAggregate user) : base($"The user account '{user}' is not confirmed.")
  {
    Data["User"] = user.ToString();
  }
}
