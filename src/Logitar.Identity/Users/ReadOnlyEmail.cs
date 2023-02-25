namespace Logitar.Identity.Users;

/// <summary>
/// Represents the email address of an user.
/// </summary>
public record ReadOnlyEmail : ReadOnlyContact
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyEmail"/> record using the specified arguments.
  /// </summary>
  /// <param name="address">The email address.</param>
  /// <param name="isVerified">A value indicating whether or not the email address is verified.</param>
  public ReadOnlyEmail(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
  }

  /// <summary>
  /// Gets the email address.
  /// </summary>
  public string Address { get; }
}
