namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The exception raised when a custom identifier conflict occurs.
/// </summary>
public class CustomIdentifierAlreadyUsedException : Exception
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public const string ErrorMessage = "The specified custom identifier is already used.";

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
  /// <param name="type">The type of the object that caused the conflict.</param>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="key">The conflicting identifier key.</param>
  /// <param name="value">The conflicting identifier value.</param>
  public CustomIdentifierAlreadyUsedException(Type type, TenantId? tenantId, string key, string value)
    : base(BuildMessage(type, tenantId, key, value))
  {
    TypeName = type.GetNamespaceQualifiedName();
    TenantId = tenantId?.Value;
    Key = key;
    Value = value;
  }

  private static string BuildMessage(Type type, TenantId? tenantId, string key, string value) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(TenantId), tenantId?.Value, "<null>")
    .AddData(nameof(Key), key)
    .AddData(nameof(Value), value)
    .Build();
}

/// <summary>
/// The typed exception raised when a custom identifier conflict occurs.
/// </summary>
public class CustomIdentifierAlreadyUsedException<T> : CustomIdentifierAlreadyUsedException
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierAlreadyUsedException{T}"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="key">The conflicting identifier key.</param>
  /// <param name="value">The conflicting identifier value.</param>
  public CustomIdentifierAlreadyUsedException(TenantId? tenantId, string key, string value)
    : base(typeof(T), tenantId, key, value)
  {
  }
}
