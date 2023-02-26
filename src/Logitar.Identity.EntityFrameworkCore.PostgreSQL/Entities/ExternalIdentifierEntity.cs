using Logitar.Identity.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing an user's external identifier.
/// </summary>
internal class ExternalIdentifierEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ExternalIdentifierEntity"/> class using the specified arguments.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="user">The user owning the external identifier.</param>
  /// <param name="actor">The actor creating the external identifier.</param>
  public ExternalIdentifierEntity(ExternalIdentifierSavedEvent e, UserEntity user, ActorEntity actor)
  {
    if (e.Value == null)
    {
      throw new ArgumentException($"The {e.Value} is required.", nameof(e));
    }

    Realm = user.Realm;
    RealmId = user.RealmId;

    User = user;
    UserId = user.UserId;

    Key = e.Key;
    Value = e.Value;

    CreatedById = e.ActorId.Value;
    CreatedBy = actor.Serialize();
    CreatedOn = e.OccurredOn;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="ExternalIdentifierEntity"/> class.
  /// </summary>
  private ExternalIdentifierEntity()
  {
  }

  /// <summary>
  /// Gets or sets the primary identifier of the external identifier.
  /// </summary>
  public int ExternalIdentifierId { get; private set; }

  /// <summary>
  /// Gets or sets the realm in which the external identifier belongs.
  /// </summary>
  public RealmEntity? Realm { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the realm in which the external identifier belongs.
  /// </summary>
  public int RealmId { get; private set; }

  /// <summary>
  /// Gets or sets the user owning this external identifier.
  /// </summary>
  public UserEntity? User { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the user owning this external identifier.
  /// </summary>
  public int UserId { get; private set; }

  /// <summary>
  /// Gets or sets the key of the external identifier.
  /// </summary>
  public string Key { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the value of the external identifier.
  /// </summary>
  public string Value { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the normalized value of the external identifier for unicity purposes.
  /// </summary>
  public string ValueNormalized
  {
    get => Value.ToUpper();
    private set { }
  }

  /// <summary>
  /// Gets or sets the identifier of the actor who created the external identifier.
  /// </summary>
  public string CreatedById { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the serialized actor who created the external identifier.
  /// </summary>
  public string CreatedBy { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the date and time when the external identifier was created.
  /// </summary>
  public DateTime CreatedOn { get; private set; }

  /// <summary>
  /// Gets or sets the identifier of the actor who updated the external identifier lastly.
  /// </summary>
  public string? UpdatedById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who updated the external identifier lastly.
  /// </summary>
  public string? UpdatedBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the external identifier was updated lastly.
  /// </summary>
  public DateTime? UpdatedOn { get; private set; }

  /// <summary>
  /// Updates the external identifier to the state of the specified event.
  /// </summary>
  /// <param name="e">The update event.</param>
  /// <param name="actor">The actor updating the external identifier.</param>
  public void Update(ExternalIdentifierSavedEvent e, ActorEntity actor)
  {
    if (e.Value == null)
    {
      throw new ArgumentException($"The {e.Value} is required.", nameof(e));
    }

    Value = e.Value;

    UpdatedById = e.ActorId.Value;
    UpdatedBy = actor.Serialize();
    UpdatedOn = e.OccurredOn;
  }

  /// <summary>
  /// Update the actors of the external identifier.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <param name="actor">The JSON serialized actor.</param>
  public void UpdateActors(string id, string actor)
  {
    if (CreatedById == id)
    {
      CreatedBy = actor;
    }

    if (UpdatedById == id)
    {
      UpdatedBy = actor;
    }
  }
}
