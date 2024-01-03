namespace Logitar.Identity.Domain.Settings;

public record PasswordSettings : IPasswordSettings
{
  public string HashingStrategy { get; set; } = "PBKDF2";
}
