using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class IdentityContext : DbContext
{
  public const string Schema = "Identity";

  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
  {
  }

  public DbSet<ActorEntity> Actors => Set<ActorEntity>();
  public DbSet<ApiKeyEntity> ApiKeys => Set<ApiKeyEntity>();
  public DbSet<ApiKeyRoleEntity> ApiKeyRoles => Set<ApiKeyRoleEntity>();
  public DbSet<BlacklistedTokenEntity> TokenBlacklist => Set<BlacklistedTokenEntity>();
  public DbSet<CustomAttributeEntity> CustomAttributes => Set<CustomAttributeEntity>();
  public DbSet<OneTimePasswordEntity> OneTimePasswords => Set<OneTimePasswordEntity>();
  public DbSet<RoleEntity> Roles => Set<RoleEntity>();
  public DbSet<SessionEntity> Sessions => Set<SessionEntity>();
  public DbSet<UserEntity> Users => Set<UserEntity>();
  public DbSet<UserIdentifierEntity> UserIdentifiers => Set<UserIdentifierEntity>();
  public DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
