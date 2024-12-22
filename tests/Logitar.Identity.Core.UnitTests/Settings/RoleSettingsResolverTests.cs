using Logitar.Identity.Contracts.Settings;
using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Core.Settings;

[Trait(Traits.Category, Categories.Unit)]
public class RoleSettingsResolverTests
{
  private readonly Dictionary<string, string?> _settings = new()
  {
    ["Identity:Role:UniqueName:AllowedCharacters"] = "abcdefghijklmnopqrstuvwxyz_"
  };

  private readonly IConfiguration _configuration;
  private readonly RoleSettingsResolverMock _resolver;

  public RoleSettingsResolverTests()
  {
    _configuration = new ConfigurationBuilder()
      .AddInMemoryCollection(_settings)
      .Build();

    _resolver = new(_configuration);
  }

  [Fact(DisplayName = "Resolve: it should resolve the role settings correctly.")]
  public void Resolve_it_should_resolve_the_role_settings_correctly()
  {
    _ = _resolver.Resolve();
    Assert.Equal(1, _resolver.ReadCounter);

    IRoleSettings settings = _resolver.Resolve();
    Assert.Equal(1, _resolver.ReadCounter);

    Assert.Equal(_settings["Identity:Role:UniqueName:AllowedCharacters"], settings.UniqueName.AllowedCharacters);
  }
}

