using Logitar.EventSourcing;
using System.Globalization;

namespace Logitar.Identity.Realms.Events;

/// <summary>
/// The base <see cref="RealmAggregate"/> save event.
/// </summary>
internal abstract record RealmSavedEvent : DomainEvent
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
  /// TODO(fpion): documentation
  /// </summary>
  public bool RequireConfirmedAccount { get; init; }
  /// <summary>
  /// TODO(fpion): documentation
  /// </summary>
  public bool RequireUniqueEmail { get; init; }

  /// <summary>
  /// Gets or sets the settings used to validate usernames in the realm.
  /// </summary>
  public UsernameSettings UsernameSettings { get; init; } = new();
  /// <summary>
  /// Gets or sets the settings used to validate passwords in the realm.
  /// </summary>
  public PasswordSettings PasswordSettings { get; init; } = new();

  /// <summary>
  /// Gets or sets the secret used to sign JSON Web tokens.
  /// </summary>
  public string JwtSecret { get; init; } = string.Empty;

  // TODO(fpion): ExternalProviders

  // TODO(fpion): User CustomAttributes Claim Mappings

  /// <summary>
  /// Gets or sets the custom attributes of the realm.
  /// </summary>
  public Dictionary<string, string> CustomAttributes { get; init; } = new();
}
