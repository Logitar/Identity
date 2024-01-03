using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class IdentityContext : DbContext
{
  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
  {
  }

  public DbSet<UserEntity> Users { get; private set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
