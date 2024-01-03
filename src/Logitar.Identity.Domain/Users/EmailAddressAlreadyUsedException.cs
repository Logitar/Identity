using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public class EmailAddressAlreadyUsedException : Exception
{
  public const string BaseMessage = "The specified email address is already used.";

  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  public string EmailAddress
  {
    get => (string)Data[nameof(EmailAddress)]!;
    private set => Data[nameof(EmailAddress)] = value;
  }

  public EmailAddressAlreadyUsedException(TenantId? tenantId, EmailUnit email)
    : base(BuildMessage(tenantId, email))
  {
    TenantId = tenantId?.Value;
    EmailAddress = email.Address;
  }

  private static string BuildMessage(TenantId? tenantId, EmailUnit email) => new ErrorMessageBuilder(BaseMessage)
    .AddData(nameof(TenantId), tenantId?.Value ?? "<null>")
    .AddData(nameof(EmailAddress), email.Address)
    .Build();
}
