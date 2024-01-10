namespace Logitar.Identity.Domain.Users;

public interface IPhone
{
  string? CountryCode { get; }
  string Number { get; }
  string? Extension { get; }
}
