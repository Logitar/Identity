using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Identity.Infrastructure.Passwords.Pbkdf2;

public record Pbkdf2Settings
{
  public const string SectionKey = "Pbkdf2";

  public KeyDerivationPrf Algorithm { get; set; } = KeyDerivationPrf.HMACSHA256;
  public int Iterations { get; set; } = 600000;
  public int SaltLength { get; set; } = 256 / 8;
  public int? HashLength { get; set; }
}
