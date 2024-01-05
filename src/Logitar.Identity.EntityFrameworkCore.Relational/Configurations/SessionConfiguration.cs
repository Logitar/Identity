using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class SessionConfiguration : AggregateConfiguration<SessionEntity>, IEntityTypeConfiguration<SessionEntity>
{
  public override void Configure(EntityTypeBuilder<SessionEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.Sessions));
    builder.HasKey(x => x.SessionId);

    builder.HasIndex(x => x.IsActive);

    builder.HasOne(x => x.User).WithMany(x => x.Sessions)
      .HasPrincipalKey(x => x.UserId).HasForeignKey(x => x.UserId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
