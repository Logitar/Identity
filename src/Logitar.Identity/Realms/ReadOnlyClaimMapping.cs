namespace Logitar.Identity.Realms;

/// <summary>
/// Represents the mapping of an user custom attribute to a claim.
/// </summary>
public record ReadOnlyClaimMapping
{
  /// <summary>
  /// Gets or sets the claim type (or name) that this mapping will output.
  /// </summary>
  public string ClaimType { get; init; } = string.Empty;
  /// <summary>
  /// Gets or sets the type of claim values that this mapping will output.
  /// </summary>
  public string? ClaimValueType { get; init; }
}
