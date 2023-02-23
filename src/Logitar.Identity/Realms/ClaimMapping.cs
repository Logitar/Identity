namespace Logitar.Identity.Realms;

/// <summary>
/// The output representation of a claim mapping.
/// </summary>
public class ClaimMapping
{
  /// <summary>
  /// Gets or sets the key of the custom attribute targeted by this mapping.
  /// </summary>
  public string Key { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the claim type (or name) that this mapping will output.
  /// </summary>
  public string ClaimType { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the type of claim values that this mapping will output.
  /// </summary>
  public string? ClaimValueType { get; set; }
}
