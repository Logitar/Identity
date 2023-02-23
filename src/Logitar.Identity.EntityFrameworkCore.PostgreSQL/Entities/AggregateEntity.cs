using Logitar.EventSourcing;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing an aggregate.
/// </summary>
public abstract class AggregateEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AggregateEntity"/> class.
  /// </summary>
  protected AggregateEntity()
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="AggregateEntity"/> class to the state of the specified event.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="actor">The actor creating the aggregate.</param>
  protected AggregateEntity(DomainEvent e, ActorEntity actor)
  {
    AggregateId = e.AggregateId.Value;
    SetVersion(e);

    CreatedById = e.ActorId.Value;
    CreatedBy = actor.Serialize();
    CreatedOn = e.OccurredOn;
  }

  /// <summary>
  /// Gets or sets the identifier of the aggregate.
  /// </summary>
  public string AggregateId { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the current version of the aggregate.
  /// </summary>
  public long Version { get; private set; }

  /// <summary>
  /// Gets or sets the identifier of the actor who created the aggregate.
  /// </summary>
  public string CreatedById { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the serialized actor who created the aggregate.
  /// </summary>
  public string CreatedBy { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the date and time when the aggregate was created.
  /// </summary>
  public DateTime CreatedOn { get; private set; }

  /// <summary>
  /// Gets or sets the identifier of the actor who updated the aggregate lastly.
  /// </summary>
  public string? UpdatedById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who updated the aggregate lastly.
  /// </summary>
  public string? UpdatedBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the aggregate was updated lastly.
  /// </summary>
  public DateTime? UpdatedOn { get; private set; }

  /// <summary>
  /// Sets the version of the aggregate to the version of the specified event.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected void SetVersion(DomainEvent e)
  {
    Version = e.Version;
  }

  /// <summary>
  /// Updates the aggregate to the state of the specified event.
  /// </summary>
  /// <param name="e">The update event.</param>
  /// <param name="actor">The actor updating the aggregate.</param>
  protected void Update(DomainEvent e, ActorEntity actor)
  {
    SetVersion(e);

    UpdatedById = e.ActorId.Value;
    UpdatedBy = actor.Serialize();
    UpdatedOn = e.OccurredOn;
  }
}
