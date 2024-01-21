using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class IdentityContext : DbContext
{
  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
  {
  }

  public DbSet<ActorEntity> Actors { get; private set; }
  public DbSet<ApiKeyRoleEntity> ApiKeyRoles { get; private set; }
  public DbSet<ApiKeyEntity> ApiKeys { get; private set; }
  public DbSet<CustomAttributeEntity> CustomAttributes { get; private set; }
  public DbSet<RoleEntity> Roles { get; private set; }
  public DbSet<SessionEntity> Sessions { get; private set; }
  public DbSet<BlacklistedTokenEntity> TokenBlacklist { get; private set; }
  public DbSet<UserIdentifierEntity> UserIdentifiers { get; private set; }
  public DbSet<UserRoleEntity> UserRoles { get; private set; }
  public DbSet<UserEntity> Users { get; private set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
