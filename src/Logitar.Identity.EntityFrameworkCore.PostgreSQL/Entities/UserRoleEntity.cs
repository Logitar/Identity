namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing a relation between an user and a role.
/// </summary>
internal class UserRoleEntity
{
  /// <summary>
  /// Gets or sets the user in the relation.
  /// </summary>
  public UserEntity? User { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the user in the relation.
  /// </summary>
  public int UserId { get; private set; }

  /// <summary>
  /// Gets or sets the role in the relation.
  /// </summary>
  public RoleEntity? Role { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the role in the relation.
  /// </summary>
  public int RoleId { get; private set; }
}
