namespace Logitar.Identity.ApiKeys;

/// <summary>
/// The API key creation input data.
/// </summary>
public record CreateApiKeyInput : SaveApiKeyInput
{
  /// <summary>
  /// Gets or sets the identifier of the realm in which this API key belongs.
  /// </summary>
  public Guid RealmId { get; set; }

  /// <summary>
  /// Gets or sets the prefix of the API key, typically two characters.
  /// </summary>
  public string Prefix { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the date and time when the API key will expire.
  /// </summary>
  public DateTime? ExpiresOn { get; set; }
}
