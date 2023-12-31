﻿using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

/// <summary>
/// The database context used for the identity system.
/// </summary>
public class IdentityContext : DbContext
{
  /// <summary>
  /// Initializes a new instance of the <see cref="IdentityContext"/> class using the specified options.
  /// </summary>
  /// <param name="options">The database context options.</param>
  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
  {
  }

  /// <summary>
  /// Gets or sets the data set of API key roles.
  /// </summary>
  internal DbSet<ApiKeyRoleEntity> ApiKeyRoles { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of API keys.
  /// </summary>
  internal DbSet<ApiKeyEntity> ApiKeys { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of external identifiers.
  /// </summary>
  internal DbSet<ExternalIdentifierEntity> ExternalIdentifiers { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of realms.
  /// </summary>
  internal DbSet<RealmEntity> Realms { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of roles.
  /// </summary>
  internal DbSet<RoleEntity> Roles { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of sessions.
  /// </summary>
  internal DbSet<SessionEntity> Sessions { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of user roles.
  /// </summary>
  internal DbSet<UserRoleEntity> UserRoles { get; private set; } = null!;
  /// <summary>
  /// Gets or sets the data set of users.
  /// </summary>
  internal DbSet<UserEntity> Users { get; private set; } = null!;

  /// <summary>
  /// Configures the database context model creation process using the specified model builder.
  /// </summary>
  /// <param name="builder">The model builder.</param>
  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
  }
}
