namespace Logitar.Identity.Contracts.Account;

public record RegisterPayload
{
  public string UniqueName { get; set; } = string.Empty;
}
