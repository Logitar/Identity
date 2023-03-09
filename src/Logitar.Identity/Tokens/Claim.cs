namespace Logitar.Identity.Tokens;

/// <summary>
/// The representation of a security claim.
/// </summary>
public record Claim
{
  /// <summary>
  /// Gets or sets the type of the claim.
  /// </summary>
  public string Type { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the value of the claim.
  /// </summary>
  public string Value { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the type of the claim's value.
  /// </summary>
  public string? ValueType { get; set; }
}
