using Logitar.Identity.ApiKeys.Events;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing an API key.
/// </summary>
internal class ApiKeyEntity : AggregateEntity, ICustomAttributes
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyEntity"/> using the specified arguments.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="realm">The realm the API key belongs to.</param>
  /// <param name="actor">The actor creating the API key.</param>
  public ApiKeyEntity(ApiKeyCreatedEvent e, RealmEntity realm, ActorEntity actor) : base(e, actor)
  {
    Realm = realm;
    RealmId = realm.RealmId;

    SecretHash = e.SecretHash;

    ExpiresOn = e.ExpiresOn;

    Apply(e);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyEntity"/> class.
  /// </summary>
  private ApiKeyEntity() : base()
  {
  }

  /// <summary>
  /// Gets or sets the primary identifier of the API key.
  /// </summary>
  public int ApiKeyId { get; private set; }

  /// <summary>
  /// Gets or sets the realm in which the API key belongs.
  /// </summary>
  public RealmEntity? Realm { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the realm in which the API key belongs.
  /// </summary>
  public int RealmId { get; private set; }

  /// <summary>
  /// Gets or sets the salted and hashed secret of the API key.
  /// </summary>
  public string SecretHash { get; private set; } = string.Empty;

  /// <summary>
  /// Gets or sets the title (or display name) of the API key.
  /// </summary>
  public string Title { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets a textual description for the API key.
  /// </summary>
  public string? Description { get; private set; }

  /// <summary>
  /// Gets or sets the date and time when the API key expires.
  /// </summary>
  public DateTime? ExpiresOn { get; private set; }

  /// <summary>
  /// Gets or sets the serialized custom attributes of the API key.
  /// </summary>
  public string? CustomAttributes { get; private set; }

  /// <summary>
  /// Gets or sets the roles (scopes) of the API key.
  /// </summary>
  public List<RoleEntity> Roles { get; private set; } = new();

  /// <summary>
  /// Updates the API key to the state of the specified event.
  /// </summary>
  /// <param name="e">The update event.</param>
  /// <param name="actor">The actor updating the API key.</param>
  public void Update(ApiKeyUpdatedEvent e, ActorEntity actor)
  {
    base.Update(e, actor);

    Apply(e);
  }

  /// <summary>
  /// Applies the specified event to the API key.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  private void Apply(ApiKeySavedEvent e)
  {
    Title = e.Title;
    Description = e.Description;

    CustomAttributes = e.CustomAttributes.Any() ? JsonSerializer.Serialize(e.CustomAttributes) : null;
  }
}
