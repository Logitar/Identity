namespace Logitar.Identity.Domain.Settings;

public record RoleSettings : IRoleSettings
{
  public IUniqueNameSettings UniqueName { get; set; } = new UniqueNameSettings();
}
