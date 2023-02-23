namespace Logitar.Identity.Roles;

/// <summary>
/// The role creation input data.
/// </summary>
public record CreateRoleInput : SaveRoleInput
{
  /// <summary>
  /// Gets or sets the identifier of the realm in which this role belongs.
  /// </summary>
  public Guid RealmId { get; set; }

  /// <summary>
  /// Gets or sets the unique name of the role (case-insensitive).
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;
}
