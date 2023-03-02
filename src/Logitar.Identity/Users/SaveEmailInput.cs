namespace Logitar.Identity.Users;

/// <summary>
/// The base email address input data.
/// </summary>
public abstract record SaveEmailInput
{
  /// <summary>
  /// Gets or sets the email address.
  /// </summary>
  public string Address { get; set; } = string.Empty;
}
