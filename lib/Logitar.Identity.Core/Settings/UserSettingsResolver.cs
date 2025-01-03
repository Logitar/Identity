﻿using Logitar.Identity.Contracts.Settings;
using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Core.Settings;

/// <summary>
/// An implementation of a role settings resolver using the application configuration.
/// </summary>
public class UserSettingsResolver : IUserSettingsResolver
{
  /// <summary>
  /// The key of the user settings configuration section.
  /// </summary>
  public const string SectionKey = "Identity:User";

  /// <summary>
  /// Gets or sets the configuration of the application.
  /// </summary>
  protected virtual IConfiguration Configuration { get; }
  /// <summary>
  /// Gets or sets the cached user settings.
  /// </summary>
  protected virtual IUserSettings? UserSettings { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserSettingsResolver"/> class.
  /// </summary>
  /// <param name="configuration">The configuration of the application.</param>
  public UserSettingsResolver(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  /// <summary>
  /// Resolves the user settings.
  /// </summary>
  /// <returns>The user settings.</returns>
  public virtual IUserSettings Resolve()
  {
    UserSettings ??= Configuration.GetSection(SectionKey).Get<UserSettings>() ?? new();
    return UserSettings;
  }
}
