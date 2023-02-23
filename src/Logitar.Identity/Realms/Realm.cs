namespace Logitar.Identity.Realms;

/// <summary>
/// The output representation of a realm.
/// </summary>
public record Realm : Aggregate
{
  /// <summary>
  /// Gets or sets the identifier of the realm.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets the unique name of the realm (case-insensitive).
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the display name of the realm.
  /// </summary>
  public string? DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the textual description of the realm.
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// Gets or sets the default locale of the realm.
  /// </summary>
  public string? DefaultLocale { get; set; }
  /// <summary>
  /// Gets or sets the URL of the realm, if it is used by an external Web application.
  /// </summary>
  public string? Url { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether or not users in this realm need a verified contact to
  /// sign-in to their account.
  /// </summary>
  public bool RequireConfirmedAccount { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not email addresses can be used by users in this
  /// realm to sign-in to their account. Unicity will be enforced upon sign-in email addresses.
  /// sign-in
  /// </summary>
  public bool RequireUniqueEmail { get; set; }

  /// <summary>
  /// Gets or sets the settings used to validate usernames in the realm.
  /// </summary>
  public UsernameSettings UsernameSettings { get; set; } = new();
  /// <summary>
  /// Gets or sets the settings used to validate passwords in the realm.
  /// </summary>
  public PasswordSettings PasswordSettings { get; set; } = new();

  /// <summary>
  /// Gets or sets the secret used to sign JSON Web tokens.
  /// </summary>
  public string JwtSecret { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the user claim mappings of the realm.
  /// </summary>
  public IEnumerable<ClaimMapping> ClaimMappings { get; set; } = Enumerable.Empty<ClaimMapping>();

  /// <summary>
  /// Gets or sets the custom attributes of the realm.
  /// </summary>
  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();

  /// <summary>
  /// Gets or sets the configuration for the Google OAuth 2.0 authentication provider.
  /// </summary>
  public GoogleOAuth2Configuration? GoogleOAuth2Configuration { get; set; }
}
