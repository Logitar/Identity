namespace Logitar.Identity.ApiKeys;

/// <summary>
/// The API key creation input data.
/// </summary>
public record CreateApiKeyInput : SaveApiKeyInput
{
  /// <summary>
  /// Gets or sets the identifier or unique name of the realm in which the API key belongs.
  /// </summary>
  public string Realm { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the prefix of the API key, typically two characters.
  /// </summary>
  public string Prefix { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the date and time when the API key will expire.
  /// </summary>
  public DateTime? ExpiresOn { get; set; }
}
