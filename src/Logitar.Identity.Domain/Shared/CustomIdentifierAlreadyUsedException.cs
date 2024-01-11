namespace Logitar.Identity.Domain.Shared;

public class CustomIdentifierAlreadyUsedException : Exception
{
  public const string ErrorMessage = "The specified custom identifier is already used.";

  public string TypeName
  {
    get => (string)Data[nameof(TypeName)]!;
    private set => Data[nameof(TypeName)] = value;
  }
  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  public string Key
  {
    get => (string)Data[nameof(Key)]!;
    private set => Data[nameof(Key)] = value;
  }
  public string Value
  {
    get => (string)Data[nameof(Value)]!;
    private set => Data[nameof(Value)] = value;
  }

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
    .AddData(nameof(TenantId), tenantId?.Value ?? "<null>")
    .AddData(nameof(Key), key)
    .AddData(nameof(Value), value)
    .Build();
}

public class CustomIdentifierAlreadyUsedException<T> : CustomIdentifierAlreadyUsedException
{
  public CustomIdentifierAlreadyUsedException(TenantId? tenantId, string key, string value)
    : base(typeof(T), tenantId, key, value)
  {
  }
}
