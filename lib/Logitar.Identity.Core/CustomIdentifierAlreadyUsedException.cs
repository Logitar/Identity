using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core;

/// <summary>
/// The exception raised when a custom identifier conflict occurs.
/// </summary>
public class CustomIdentifierAlreadyUsedException : IdentityException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified custom identifier is already used.";

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
  /// Gets or sets the identifier of the entity which already owns the custom identifier.
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
  /// Gets or sets the conflicting identifier key.
  /// </summary>
  public string Key
  {
    get => (string)Data[nameof(Key)]!;
    private set => Data[nameof(Key)] = value;
  }
  /// <summary>
  /// Gets or sets the conflicting identifier value.
  /// </summary>
  public string Value
  {
    get => (string)Data[nameof(Value)]!;
    private set => Data[nameof(Value)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="user">The user which caused the conflict.</param>
  /// <param name="conflict">The user which already owns the custom identifier.</param>
  /// <param name="key">The conflicting identifier key.</param>
  /// <param name="value">The conflicting identifier value.</param>
  public CustomIdentifierAlreadyUsedException(User user, User conflict, Identifier key, CustomIdentifier value) : this(typeof(User), user.TenantId, conflict.EntityId, user.EntityId, key, value)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="type">The type of the object that caused the conflict.</param>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="conflictId">The identifier of the entity which already owns the custom identifier.</param>
  /// <param name="entityId">The identifier of the entity which caused the conflict.</param>
  /// <param name="key">The conflicting identifier key.</param>
  /// <param name="value">The conflicting identifier value.</param>
  public CustomIdentifierAlreadyUsedException(Type type, TenantId? tenantId, EntityId conflictId, EntityId entityId, Identifier key, CustomIdentifier value) : base(BuildMessage(type, tenantId, conflictId, entityId, key, value))
  {
    TypeName = type.GetNamespaceQualifiedName();
    TenantId = tenantId?.Value;
    ConflictId = conflictId.Value;
    EntityId = entityId.Value;
    Key = key.Value;
    Value = value.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="type">The type of the object that caused the conflict.</param>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="conflictId">The identifier of the entity which already owns the custom identifier.</param>
  /// <param name="entityId">The identifier of the entity which caused the conflict.</param>
  /// <param name="key">The conflicting identifier key.</param>
  /// <param name="value">The conflicting identifier value.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(Type type, TenantId? tenantId, EntityId conflictId, EntityId entityId, Identifier key, CustomIdentifier value) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(TenantId), tenantId?.Value, "<null>")
    .AddData(nameof(ConflictId), conflictId)
    .AddData(nameof(EntityId), entityId)
    .AddData(nameof(Key), key)
    .AddData(nameof(Value), value)
    .Build();
}
