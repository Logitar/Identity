using Logitar.Identity.Roles.Events;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing a role.
/// </summary>
internal class RoleEntity : AggregateEntity, ICustomAttributes
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleEntity"/> using the specified arguments.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="realm">The realm the role belongs to.</param>
  /// <param name="actor">The actor creating the role.</param>
  public RoleEntity(RoleCreatedEvent e, RealmEntity realm, ActorEntity actor) : base(e, actor)
  {
    Realm = realm;
    RealmId = realm.RealmId;

    UniqueName = e.UniqueName;

    Apply(e);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleEntity"/> class.
  /// </summary>
  private RoleEntity() : base()
  {
  }

  /// <summary>
  /// Gets or sets the primary identifier of the role.
  /// </summary>
  public int RoleId { get; private set; }

  /// <summary>
  /// Gets or sets the realm in which the role belongs.
  /// </summary>
  public RealmEntity? Realm { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the realm in which the role belongs.
  /// </summary>
  public int RealmId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the role (case-insensitive).
  /// </summary>
  public string UniqueName { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the normalized unique name of the role (case-insensitive).
  /// </summary>
  public string UniqueNameNormalized
  {
    get => UniqueName.ToUpper();
    private set { }
  }
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public string? DisplayName { get; private set; }
  /// <summary>
  /// Gets or sets a textual description for the role.
  /// </summary>
  public string? Description { get; private set; }

  /// <summary>
  /// Gets or sets the serialized custom attributes of the role.
  /// </summary>
  public string? CustomAttributes { get; private set; }

  /// <summary>
  /// Gets or sets the list of API keys in this role.
  /// </summary>
  public List<ApiKeyEntity> ApiKeys { get; private set; } = new();

  /// <summary>
  /// Gets or sets the list of users in this role.
  /// </summary>
  public List<UserEntity> Users { get; private set; } = new();

  /// <summary>
  /// Updates the role to the state of the specified event.
  /// </summary>
  /// <param name="e">The update event.</param>
  /// <param name="actor">The actor updating the role.</param>
  public void Update(RoleUpdatedEvent e, ActorEntity actor)
  {
    base.Update(e, actor);

    Apply(e);
  }

  /// <summary>
  /// Applies the specified event to the role.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  private void Apply(RoleSavedEvent e)
  {
    DisplayName = e.DisplayName;
    Description = e.Description;

    CustomAttributes = e.CustomAttributes.Any() ? JsonSerializer.Serialize(e.CustomAttributes) : null;
  }
}
