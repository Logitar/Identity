using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Configurations;

/// <summary>
/// The model configuration used to configure the <see cref="BlacklistedJwtEntity"/> class.
/// </summary>
internal class BlacklistedJwtConfiguration : IEntityTypeConfiguration<BlacklistedJwtEntity>
{
  /// <summary>
  /// Configures the database model using the specified type builder.
  /// </summary>
  /// <param name="builder">The type builder.</param>
  public void Configure(EntityTypeBuilder<BlacklistedJwtEntity> builder)
  {
    builder.HasKey(x => x.BlacklistedJwtId);

    builder.HasIndex(x => x.ExpiresOn);
    builder.HasIndex(x => x.Id).IsUnique();
  }
}
