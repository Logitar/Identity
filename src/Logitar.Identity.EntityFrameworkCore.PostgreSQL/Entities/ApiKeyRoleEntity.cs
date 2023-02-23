namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing a relation between an API key and a role.
/// </summary>
internal class ApiKeyRoleEntity
{
  /// <summary>
  /// Gets or sets the API key in the relation.
  /// </summary>
  public ApiKeyEntity? ApiKey { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the API key in the relation.
  /// </summary>
  public int ApiKeyId { get; private set; }

  /// <summary>
  /// Gets or sets the role in the relation.
  /// </summary>
  public RoleEntity? Role { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the role in the relation.
  /// </summary>
  public int RoleId { get; private set; }
}
