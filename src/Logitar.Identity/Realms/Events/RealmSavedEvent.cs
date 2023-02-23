using Logitar.EventSourcing;
using System.Globalization;

namespace Logitar.Identity.Realms.Events;

/// <summary>
/// The base <see cref="RealmAggregate"/> save event.
/// </summary>
public abstract record RealmSavedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the display name of the realm.
  /// </summary>
  public string? DisplayName { get; init; }
  /// <summary>
  /// Gets or sets the textual description of the realm.
  /// </summary>
  public string? Description { get; init; }

  /// <summary>
  /// Gets or sets the default locale of the realm.
  /// </summary>
  public CultureInfo? DefaultLocale { get; init; }
  /// <summary>
  /// Gets or sets the URL of the realm, if it is used by an external Web application.
  /// </summary>
  public string? Url { get; init; }

  /// <summary>
  /// Gets or sets a value indicating whether or not users in this realm need a verified contact to
  /// sign-in to their account.
  /// </summary>
  public bool RequireConfirmedAccount { get; init; }
  /// <summary>
  /// Gets or sets a value indicating whether or not email addresses can be used by users in this
  /// realm to sign-in to their account. Unicity will be enforced upon sign-in email addresses.
  /// </summary>
  public bool RequireUniqueEmail { get; init; }

  /// <summary>
  /// Gets or sets the settings used to validate usernames in the realm.
  /// </summary>
  public ReadOnlyUsernameSettings UsernameSettings { get; init; } = new();
  /// <summary>
  /// Gets or sets the settings used to validate passwords in the realm.
  /// </summary>
  public ReadOnlyPasswordSettings PasswordSettings { get; init; } = new();

  /// <summary>
  /// Gets or sets the secret used to sign JSON Web tokens.
  /// </summary>
  public string JwtSecret { get; init; } = string.Empty;

  /// <summary>
  /// Gets or sets the user claim mappings of the realm.
  /// </summary>
  public Dictionary<string, ReadOnlyClaimMapping> ClaimMappings { get; init; } = new();

  /// <summary>
  /// Gets or sets the custom attributes of the realm.
  /// </summary>
  public Dictionary<string, string> CustomAttributes { get; init; } = new();

  /// <summary>
  /// Gets or sets the Google OAuth 2.0 provider authentication configuration.
  /// </summary>
  public ReadOnlyGoogleOAuth2Configuration? GoogleOAuth2Configuration { get; init; }
}
