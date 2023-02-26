using Logitar.Identity.Actors;
using Logitar.Identity.Users;

namespace Logitar.Identity.Sessions;

/// <summary>
/// The output representation of an user session.
/// </summary>
public record Session : Aggregate
{
  /// <summary>
  /// Gets or sets or sets the identifier of the user session.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets the user to whom the session belongs.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets the refresh token of the session.
  /// </summary>
  public string? RefreshToken { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the session is persistent. A session is
  /// persistent if a refresh token has been issued.
  /// </summary>
  public bool IsPersistent { get; set; }

  /// <summary>
  /// Gets or sets the actor who signed-out the session.
  /// </summary>
  public Actor? SignedOutBy { get; set; }
  /// <summary>
  /// Gets or sets the date and time when the session was signed-out.
  /// </summary>
  public DateTime? SignedOutOn { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the session is active. A session is unactive if
  /// it has been signed-out.
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Gets or sets the custom attributes of the session.
  /// </summary>
  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();
}
