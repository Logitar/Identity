﻿using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Domain.Settings;

/// <summary>
/// An implementation of a role settings resolver using the application configuration.
/// </summary>
public class RoleSettingsResolver : IRoleSettingsResolver
{
  /// <summary>
  /// Gets or sets the configuration of the application.
  /// </summary>
  protected virtual IConfiguration Configuration { get; }
  /// <summary>
  /// Gets or sets the cached role settings.
  /// </summary>
  protected virtual IRoleSettings? RoleSettings { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleSettingsResolver"/> class.
  /// </summary>
  /// <param name="configuration">The configuration of the application.</param>
  public RoleSettingsResolver(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  /// <summary>
  /// Resolves the role settings.
  /// </summary>
  /// <returns>The role settings.</returns>
  public virtual IRoleSettings Resolve()
  {
    RoleSettings ??= Configuration.GetSection("Role").Get<RoleSettings>() ?? new();
    return RoleSettings;
  }
}
