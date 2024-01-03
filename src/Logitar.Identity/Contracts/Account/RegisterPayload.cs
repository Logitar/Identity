namespace Logitar.Identity.Contracts.Account;

public record RegisterPayload
{
  public string UniqueName { get; set; } = string.Empty;
  // TODO(fpion): Password

  public string? EmailAddress { get; set; }

  // TODO(fpion): FirstName
  // TODO(fpion): LastName
}
