using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core;

/// <summary>
/// The exception raised when an unique name conflict occurs.
/// </summary>
public class UniqueNameAlreadyUsedException : IdentityException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified unique name is already used.";

  /// <summary>
  /// Gets or sets the name of the type of the object that caused the conflict.
  /// </summary>
  public string TypeName
  {
    get => (string)Data[nameof(TypeName)]!;
    private set => Data[nameof(TypeName)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the conflict occurred.
  /// </summary>
  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the entity which already owns the unique name.
  /// </summary>
  public string ConflictId
  {
    get => (string)Data[nameof(ConflictId)]!;
    private set => Data[nameof(ConflictId)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the entity which caused the conflict.
  /// </summary>
  public string EntityId
  {
    get => (string)Data[nameof(EntityId)]!;
    private set => Data[nameof(EntityId)] = value;
  }
  /// <summary>
  /// Gets or sets the conflicting unique name.
  /// </summary>
  public string UniqueName
  {
    get => (string)Data[nameof(UniqueName)]!;
    private set => Data[nameof(UniqueName)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException" /> class.
  /// </summary>
  /// <param name="role">The role which caused the conflict.</param>
  /// <param name="conflict">The role which already owns the unique name.</param>
  public UniqueNameAlreadyUsedException(Role role, Role conflict) : this(role.GetType(), role.TenantId, conflict.EntityId, role.EntityId, role.UniqueName)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException" /> class.
  /// </summary>
  /// <param name="user">The user which caused the conflict.</param>
  /// <param name="conflict">The user which already owns the unique name.</param>
  public UniqueNameAlreadyUsedException(User user, User conflict) : this(user.GetType(), user.TenantId, conflict.EntityId, user.EntityId, user.UniqueName)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException" /> class.
  /// </summary>
  /// <param name="type">The type of the object that caused the conflict.</param>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="conflictId">The identifier of the entity which already owns the unique name.</param>
  /// <param name="entityId">The identifier of the entity which caused the conflict.</param>
  /// <param name="uniqueName">The conflicting unique name.</param>
  public UniqueNameAlreadyUsedException(Type type, TenantId? tenantId, EntityId conflictId, EntityId entityId, UniqueName uniqueName) : base(BuildMessage(type, tenantId, conflictId, entityId, uniqueName))
  {
    TypeName = type.GetNamespaceQualifiedName();
    TenantId = tenantId?.Value;
    ConflictId = conflictId.Value;
    EntityId = entityId.Value;
    UniqueName = uniqueName.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="type">The type of the object that caused the conflict.</param>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="conflictId">The identifier of the entity which already owns the unique name.</param>
  /// <param name="entityId">The identifier of the entity which caused the conflict.</param>
  /// <param name="uniqueName">The conflicting unique name.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(Type type, TenantId? tenantId, EntityId conflictId, EntityId entityId, UniqueName uniqueName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(TenantId), tenantId, "<null>")
    .AddData(nameof(ConflictId), conflictId)
    .AddData(nameof(EntityId), entityId)
    .AddData(nameof(UniqueName), uniqueName.Value)
    .Build();
}
