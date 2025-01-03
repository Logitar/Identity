﻿using Logitar.Identity.Contracts.Settings;
using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Core.Settings;

[Trait(Traits.Category, Categories.Unit)]
public class UserSettingsResolverTests
{
  private readonly Dictionary<string, string?> _settings = new()
  {
    ["Identity:User:Password:HashingStrategy"] = "CUSTOM",
    ["Identity:User:UniqueName:AllowedCharacters"] = "abcdefghijklmnopqrstuvwxyz0123456789@",
    ["Identity:User:RequireUniqueEmail"] = "true"
  };

  private readonly IConfiguration _configuration;
  private readonly UserSettingsResolverMock _resolver;

  public UserSettingsResolverTests()
  {
    _configuration = new ConfigurationBuilder()
      .AddInMemoryCollection(_settings)
      .Build();

    _resolver = new(_configuration);
  }

  [Fact(DisplayName = "Resolve: it should resolve the user settings correctly.")]
  public void Resolve_it_should_resolve_the_user_settings_correctly()
  {
    _ = _resolver.Resolve();
    Assert.Equal(1, _resolver.ReadCounter);

    IUserSettings settings = _resolver.Resolve();
    Assert.Equal(1, _resolver.ReadCounter);

    Assert.Equal(_settings["Identity:User:Password:HashingStrategy"], settings.Password.HashingStrategy);
    Assert.Equal(_settings["Identity:User:UniqueName:AllowedCharacters"], settings.UniqueName.AllowedCharacters);
    Assert.True(settings.RequireUniqueEmail);
  }
}
