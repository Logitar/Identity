namespace Logitar.Identity.Domain.Shared;

public class UniqueNameAlreadyUsedException : Exception
{
  public const string ErrorMessage = "The specified unique name is already used.";

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
  public string UniqueName
  {
    get => (string)Data[nameof(UniqueName)]!;
    private set => Data[nameof(UniqueName)] = value;
  }

  public UniqueNameAlreadyUsedException(Type type, TenantId? tenantId, UniqueNameUnit uniqueName)
    : base(BuildMessage(type, tenantId, uniqueName))
  {
    TypeName = type.GetNamespaceQualifiedName();
    TenantId = tenantId?.Value;
    UniqueName = uniqueName.Value;
  }

  private static string BuildMessage(Type type, TenantId? tenantId, UniqueNameUnit uniqueName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(TenantId), tenantId?.Value ?? "<null>")
    .AddData(nameof(UniqueName), uniqueName.Value)
    .Build();
}

public class UniqueNameAlreadyUsedException<T> : UniqueNameAlreadyUsedException
{
  public UniqueNameAlreadyUsedException(TenantId? tenantId, UniqueNameUnit uniqueName)
    : base(typeof(T), tenantId, uniqueName)
  {
  }
}
