namespace Logitar.Identity.Users;

/// <summary>
/// The output representation of an email address.
/// </summary>
public record Email : Contact
{
  /// <summary>
  /// Gets or sets the email address.
  /// </summary>
  public string Address { get; set; } = string.Empty;
}
