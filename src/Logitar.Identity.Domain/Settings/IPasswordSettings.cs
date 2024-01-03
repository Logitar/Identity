namespace Logitar.Identity.Domain.Settings;

public interface IPasswordSettings
{
  string HashingStrategy { get; }
}
