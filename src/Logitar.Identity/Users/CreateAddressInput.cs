namespace Logitar.Identity.Users;

/// <summary>
/// The postal address creation input data.
/// </summary>
public record CreateAddressInput : SaveAddressInput
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the postal address is verified.
  /// </summary>
  public bool IsVerified { get; set; }
}
