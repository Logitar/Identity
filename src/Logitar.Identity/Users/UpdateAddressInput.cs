namespace Logitar.Identity.Users;

/// <summary>
/// The postal address update input data.
/// </summary>
public record UpdateAddressInput : SaveAddressInput
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the postal address will be verified. If false,
  /// the postal address will be unverified if it is modified.
  /// </summary>
  public bool Verify { get; set; }
}
