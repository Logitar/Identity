using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Accounts;
using Logitar.Identity.Realms.Events;
using Logitar.Identity.Realms.Validators;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Logitar.Identity.Realms;

/// <summary>
/// The domain aggregate representing a realm. Realms contain configurations as well as metadata. All
/// objects in the identity system, such as Users and Roles, are scoped to a realm. In a single-tenant
/// application, only one realm can be used. In a multi-tenant application, a realm could be used for
/// each tenant, and an additional realm could be used as the administration identity database.
/// </summary>
public class RealmAggregate : AggregateRoot
{
  /// <summary>
  /// The user claim mappings of the realm.
  /// </summary>
  private readonly Dictionary<string, ReadOnlyClaimMapping> _claimMappings = new();
  /// <summary>
  /// The custom attributes of the realm.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();
  /// <summary>
  /// The external authentication provider configurations of the realm.
  /// </summary>
  private readonly Dictionary<ExternalProvider, ExternalProviderConfiguration> _externalProviders = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="RealmAggregate"/> class using the specified arguments.
  /// </summary>
  /// <param name="id">The identifier of the realm.</param>
  public RealmAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="RealmAggregate"/> class using the specified arguments.
  /// </summary>
  /// <param name="actorId">The identifier of the actor creating the realm.</param>
  /// <param name="uniqueName">The unique name of the realm.</param>
  /// <param name="displayName">The display name of the realm.</param>
  /// <param name="description">The textual description of the realm.</param>
  /// <param name="defaultLocale">The default locale of the realm.</param>
  /// <param name="url">The URL of the realm, if it is used by an external Web application.</param>
  /// <param name="requireConfirmedAccount">Gets or sets a value indicating whether or not users in this realm need a verified contact to sign-in to their account.</param>
  /// <param name="requireUniqueEmail">Gets or sets a value indicating whether or not email addresses can be used by users in this realm to sign-in to their account. Unicity will be enforced upon sign-in email addresses.</param>
  /// <param name="usernameSettings">The settings used to validate usernames in the realm.</param>
  /// <param name="passwordSettings">The settings used to validate passwords in the realm.</param>
  /// <param name="jwtSecret">The secret used to sign JSON Web tokens.</param>
  /// <param name="claimMappings">The user claim mappings of the realm.</param>
  /// <param name="customAttributes">The custom attributes of the realm.</param>
  /// <param name="externalProviders">The external authentication provider configurations of the realm.</param>
  public RealmAggregate(AggregateId actorId, string uniqueName, string? displayName = null,
    string? description = null, CultureInfo? defaultLocale = null, string? url = null,
    bool requireConfirmedAccount = false, bool requireUniqueEmail = false,
    ReadOnlyUsernameSettings? usernameSettings = null, ReadOnlyPasswordSettings? passwordSettings = null,
    string? jwtSecret = null, Dictionary<string, ReadOnlyClaimMapping>? claimMappings = null,
    Dictionary<string, string>? customAttributes = null,
    Dictionary<ExternalProvider, ExternalProviderConfiguration>? externalProviders = null)
  {
    externalProviders ??= new();
    ReadOnlyGoogleOAuth2Configuration? googleOAuth2Configuration = null;
    if (externalProviders.TryGetValue(ExternalProvider.GoogleOAuth2, out ExternalProviderConfiguration? configuration))
    {
      googleOAuth2Configuration = (ReadOnlyGoogleOAuth2Configuration)configuration;
    }

    RealmCreatedEvent e = new()
    {
      ActorId = actorId,
      UniqueName = uniqueName.Trim(),
      DisplayName = displayName?.CleanTrim(),
      Description = description?.CleanTrim(),
      DefaultLocale = defaultLocale,
      Url = url?.CleanTrim(),
      RequireConfirmedAccount = requireConfirmedAccount,
      RequireUniqueEmail = requireUniqueEmail,
      UsernameSettings = usernameSettings ?? new(),
      PasswordSettings = passwordSettings ?? new(),
      JwtSecret = jwtSecret ?? GenerateJwtSecret(),
      ClaimMappings = claimMappings ?? new(),
      CustomAttributes = customAttributes ?? new(),
      GoogleOAuth2Configuration = googleOAuth2Configuration
    };
    new RealmCreatedValidator().ValidateAndThrow(e);

    ApplyChange(e);
  }

  /// <summary>
  /// Gets or sets the unique name of the realm (case-insensitive).
  /// </summary>
  public string UniqueName { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the display name of the realm.
  /// </summary>
  public string? DisplayName { get; private set; }
  /// <summary>
  /// Gets or sets the textual description of the realm.
  /// </summary>
  public string? Description { get; private set; }

  /// <summary>
  /// Gets or sets the default locale of the realm.
  /// </summary>
  public CultureInfo? DefaultLocale { get; private set; }
  /// <summary>
  /// Gets or sets the URL of the realm, if it is used by an external Web application.
  /// </summary>
  public string? Url { get; private set; }

  /// <summary>
  /// Gets or sets a value indicating whether or not users in this realm need a verified contact to
  /// sign-in to their account.
  /// </summary>
  public bool RequireConfirmedAccount { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not email addresses can be used by users in this
  /// realm to sign-in to their account. Unicity will be enforced upon sign-in email addresses.
  /// sign-in
  /// </summary>
  public bool RequireUniqueEmail { get; private set; }

  /// <summary>
  /// Gets or sets the settings used to validate usernames in the realm.
  /// </summary>
  public ReadOnlyUsernameSettings UsernameSettings { get; private set; } = new();
  /// <summary>
  /// Gets or sets the settings used to validate passwords in the realm.
  /// </summary>
  public ReadOnlyPasswordSettings PasswordSettings { get; private set; } = new();

  /// <summary>
  /// Gets or sets the secret used to sign JSON Web tokens.
  /// </summary>
  public string JwtSecret { get; private set; } = string.Empty;

  /// <summary>
  /// Gets the user claim mappings of the realm.
  /// </summary>
  public IReadOnlyDictionary<string, ReadOnlyClaimMapping> ClaimMappings => _claimMappings.AsReadOnly();

  /// <summary>
  /// Gets the custom attributes of the realm.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Gets the external authentication provider configurations of the realm.
  /// </summary>
  public IReadOnlyDictionary<ExternalProvider, ExternalProviderConfiguration> ExternalProviders => _externalProviders.AsReadOnly();

  /// <summary>
  /// Applies the specified event to the realm.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(RealmCreatedEvent e)
  {
    UniqueName = e.UniqueName;

    Apply((RealmSavedEvent)e);
  }

  /// <summary>
  /// Deletes the realm.
  /// </summary>
  /// <param name="actorId">The identifier of the actor deleting the realm.</param>
  public void Delete(AggregateId actorId)
  {
    ApplyChange(new RealmDeletedEvent
    {
      ActorId = actorId
    });
  }
  /// <summary>
  /// Applies the specified event to the realm.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(RealmDeletedEvent e)
  {
  }

  /// <summary>
  /// Updates the realm.
  /// </summary>
  /// <param name="actorId">The identifier of the actor updating the realm.</param>
  /// <param name="displayName">The display name of the realm.</param>
  /// <param name="description">The textual description of the realm.</param>
  /// <param name="defaultLocale">The default locale of the realm.</param>
  /// <param name="url">The URL of the realm, if it is used by an external Web application.</param>
  /// <param name="requireConfirmedAccount">Gets or sets a value indicating whether or not users in this realm need a verified contact to sign-in to their account.</param>
  /// <param name="requireUniqueEmail">Gets or sets a value indicating whether or not email addresses can be used by users in this realm to sign-in to their account. Unicity will be enforced upon sign-in email addresses.</param>
  /// <param name="usernameSettings">The settings used to validate usernames in the realm.</param>
  /// <param name="passwordSettings">The settings used to validate passwords in the realm.</param>
  /// <param name="jwtSecret">The secret used to sign JSON Web tokens.</param>
  /// <param name="claimMappings">The user claim mappings of the realm.</param>
  /// <param name="customAttributes">The custom attributes of the realm.</param>
  /// <param name="externalProviders">The external authentication provider configurations of the realm.</param>
  public void Update(AggregateId actorId, string? displayName, string? description,
    CultureInfo? defaultLocale, string? url, bool requireConfirmedAccount, bool requireUniqueEmail,
    ReadOnlyUsernameSettings? usernameSettings, ReadOnlyPasswordSettings? passwordSettings,
    string? jwtSecret, Dictionary<string, ReadOnlyClaimMapping>? claimMappings,
    Dictionary<string, string>? customAttributes,
    Dictionary<ExternalProvider, ExternalProviderConfiguration>? externalProviders)
  {
    externalProviders ??= new();
    ReadOnlyGoogleOAuth2Configuration? googleOAuth2Configuration = null;
    if (externalProviders.TryGetValue(ExternalProvider.GoogleOAuth2, out ExternalProviderConfiguration? configuration))
    {
      googleOAuth2Configuration = (ReadOnlyGoogleOAuth2Configuration)configuration;
    }

    RealmUpdatedEvent e = new()
    {
      ActorId = actorId,
      DisplayName = displayName?.CleanTrim(),
      Description = description?.CleanTrim(),
      DefaultLocale = defaultLocale,
      Url = url?.CleanTrim(),
      RequireConfirmedAccount = requireConfirmedAccount,
      RequireUniqueEmail = requireUniqueEmail,
      UsernameSettings = usernameSettings ?? new(),
      PasswordSettings = passwordSettings ?? new(),
      JwtSecret = jwtSecret ?? GenerateJwtSecret(),
      ClaimMappings = claimMappings ?? new(),
      CustomAttributes = customAttributes ?? new(),
      GoogleOAuth2Configuration = googleOAuth2Configuration
    };
    new RealmUpdatedValidator().ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the realm.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(RealmUpdatedEvent e)
  {
    Apply((RealmSavedEvent)e);
  }

  /// <summary>
  /// Applies the specified event to the realm.
  /// </summary>
  /// <param name="e">The domain event.</param>
  private void Apply(RealmSavedEvent e)
  {
    DisplayName = e.DisplayName;
    Description = e.Description;

    DefaultLocale = e.DefaultLocale;
    Url = e.Url;

    RequireConfirmedAccount = e.RequireConfirmedAccount;
    RequireUniqueEmail = e.RequireUniqueEmail;

    UsernameSettings = e.UsernameSettings;
    PasswordSettings = e.PasswordSettings;

    JwtSecret = e.JwtSecret;

    _claimMappings.Clear();
    _claimMappings.AddRange(e.ClaimMappings);

    _customAttributes.Clear();
    _customAttributes.AddRange(e.CustomAttributes);

    _externalProviders.Clear();
    if (e.GoogleOAuth2Configuration != null)
    {
      _externalProviders[ExternalProvider.GoogleOAuth2] = e.GoogleOAuth2Configuration;
    }
  }

  /// <summary>
  /// Returns a string that represents the realm.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";

  /// <summary>
  /// Generates a random JSON Web Token secret of the specified length.
  /// </summary>
  /// <param name="length">The secret length, in bytes.</param>
  /// <returns>The ASCII string representation of the secret­.</returns>
  private static string GenerateJwtSecret(int length = 256 / 8)
  {
    byte[] bytes;
    do
    {
      bytes = RandomNumberGenerator.GetBytes(length * 3).Where(b => b > 32 && b < 127).Take(length).ToArray();
    } while (bytes.Length < length);

    return Encoding.ASCII.GetString(bytes);
  }
}
