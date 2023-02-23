namespace Logitar.Identity.Actors;

public record Actor
{
  /// <summary>
  /// Gets or sets the identifier of the actor.
  /// </summary>
  public string Id { get; set; } = "SYSTEM";
  /// <summary>
  /// Gets or sets the type of the actor.
  /// </summary>
  public ActorType Type { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the actor is deleted.
  /// </summary>
  public bool IsDeleted { get; set; }

  /// <summary>
  /// Gets or sets the display name of the actor.
  /// </summary>
  public string DisplayName { get; set; } = "System";
  /// <summary>
  /// Gets or sets the email address of the actor.
  /// </summary>
  public string? Email { get; set; }
  /// <summary>
  /// Gets or sets a link (URL) to the picture of the user.
  /// </summary>
  public string? Picture { get; set; }
}
