namespace Logitar.Identity.Roles;

/// <summary>
/// The role creation input data.
/// </summary>
public record CreateRoleInput : SaveRoleInput
{
  /// <summary>
  /// Gets or sets the identifier or unique name of the realm in which the role belongs.
  /// </summary>
  public string Realm { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the unique name of the role (case-insensitive).
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;
}
