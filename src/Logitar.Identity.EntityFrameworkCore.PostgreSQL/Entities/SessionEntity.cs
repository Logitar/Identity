using Logitar.Identity.Sessions.Events;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing an user session.
/// </summary>
internal class SessionEntity : AggregateEntity, ICustomAttributes
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionEntity"/> class using the specified arguments.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="user">The user to whom the session belongs.</param>
  public SessionEntity(SessionCreatedEvent e, UserEntity user) : base(e, new ActorEntity(user))
  {
    User = user;
    UserId = user.UserId;

    KeyHash = e.KeyHash;

    IsActive = true;

    CustomAttributes = e.CustomAttributes.Any() ? JsonSerializer.Serialize(e.CustomAttributes) : null;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionEntity"/> class.
  /// </summary>
  private SessionEntity()
  {
  }

  /// <summary>
  /// Gets or sets the primary identifier of the user session.
  /// </summary>
  public int SessionId { get; private set; }

  /// <summary>
  /// Gets or sets the user to whom the session belongs.
  /// </summary>
  public UserEntity? User { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the user to whom the session belongs.
  /// </summary>
  public int UserId { get; private set; }

  /// <summary>
  /// Gets or sets the salted and hashed key of the session.
  /// </summary>
  public string? KeyHash { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the session is persistent. A session is
  /// persistent if a refresh token has been issued.
  /// </summary>
  public bool IsPersistent
  {
    get => KeyHash != null;
    private set { }
  }

  /// <summary>
  /// Gets or sets the identifier of the actor who signed-out the session.
  /// </summary>
  public string? SignedOutById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who signed-out the session.
  /// </summary>
  public string? SignedOutBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the user session was signed-out.
  /// </summary>
  public DateTime? SignedOutOn { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the session is active. A session is unactive if
  /// it has been signed-out.
  /// </summary>
  public bool IsActive { get; private set; }

  /// <summary>
  /// Gets or sets the custom attributes of the user session.
  /// </summary>
  public string? CustomAttributes { get; private set; }

  /// <summary>
  /// Update the actors of the user session.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <param name="actor">The JSON serialized actor.</param>
  public override void UpdateActors(string id, string actor)
  {
    base.UpdateActors(id, actor);

    if (SignedOutById == id)
    {
      SignedOutBy = actor;
    }
  }
}
