using Logitar.Identity.Realms.Events;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing a realm.
/// </summary>
internal class RealmEntity : AggregateEntity, ICustomAttributes
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmEntity"/> to the state of the specified event.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="actor">The actor creating the realm.</param>
  public RealmEntity(RealmCreatedEvent e, ActorEntity actor) : base(e, actor)
  {
    UniqueName = e.UniqueName;

    Apply(e);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmEntity"/> class.
  /// </summary>
  private RealmEntity() : base()
  {
  }

  /// <summary>
  /// Gets or sets the primary identifier of the realm.
  /// </summary>
  public int RealmId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the realm (case-insensitive).
  /// </summary>
  public string UniqueName { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the unique name of the realm (case-insensitive).
  /// </summary>
  public string UniqueNameNormalized
  {
    get => UniqueName.ToUpper();
    private set { }
  }
  /// <summary>
  /// Gets or sets the display name of the realm.
  /// </summary>
  public string? DisplayName { get; private set; }
  /// <summary>
  /// Gets or sets the textual description for the realm.
  /// </summary>
  public string? Description { get; private set; }

  /// <summary>
  /// Gets or sets the default locale of the realm.
  /// </summary>
  public string? DefaultLocale { get; private set; }
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
  /// realm to sign-in to their account. Unicity will be enforcedupon sign-in email addresses.
  /// </summary>
  public bool RequireUniqueEmail { get; private set; }

  /// <summary>
  /// Gets or sets the serialized settings used to validate usernames in this realm.
  /// </summary>
  public string UsernameSettings { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the serialized settings used to validate passwords in this realm.
  /// </summary>
  public string PasswordSettings { get; private set; } = string.Empty;

  /// <summary>
  /// Gets or sets the secret used to sign JSON Web Tokens.
  /// </summary>
  public string JwtSecret { get; private set; } = string.Empty;

  /// <summary>
  /// Gets or sets the serialized user claim mappings of the realm.
  /// </summary>
  public string? ClaimMappings { get; private set; }

  /// <summary>
  /// Gets or sets the serialized custom attributes of the realm.
  /// </summary>
  public string? CustomAttributes { get; private set; }

  /// <summary>
  /// Gets or sets the Google OAuth 2.0 provider authentication configuration.
  /// </summary>
  public string? GoogleOAuth2Configuration { get; private set; }

  /// <summary>
  /// Gets or sets the list of roles in this realm.
  /// </summary>
  public List<RoleEntity> Roles { get; private set; } = new();

  /// <summary>
  /// Updates the realm to the state of the specified event.
  /// </summary>
  /// <param name="e">The update event.</param>
  /// <param name="actor">The actor updating the realm.</param>
  public void Update(RealmUpdatedEvent e, ActorEntity actor)
  {
    base.Update(e, actor);

    Apply(e);
  }

  /// <summary>
  /// Applies the specified event to the realm.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  private void Apply(RealmSavedEvent e)
  {
    DisplayName = e.DisplayName;
    Description = e.Description;

    DefaultLocale = e.DefaultLocale?.Name;
    Url = e.Url;

    RequireConfirmedAccount = e.RequireConfirmedAccount;
    RequireUniqueEmail = e.RequireUniqueEmail;

    UsernameSettings = JsonSerializer.Serialize(e.UsernameSettings);
    PasswordSettings = JsonSerializer.Serialize(e.PasswordSettings);

    JwtSecret = e.JwtSecret;

    ClaimMappings = e.ClaimMappings.Any() ? JsonSerializer.Serialize(e.ClaimMappings) : null;

    CustomAttributes = e.CustomAttributes.Any() ? JsonSerializer.Serialize(e.CustomAttributes) : null;

    GoogleOAuth2Configuration = e.GoogleOAuth2Configuration == null ? null : JsonSerializer.Serialize(e.GoogleOAuth2Configuration);
  }
}
