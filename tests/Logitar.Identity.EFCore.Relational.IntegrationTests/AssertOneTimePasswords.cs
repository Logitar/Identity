using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

internal static class AssertOneTimePasswords
{
  public static void AreEqual(OneTimePasswordAggregate? oneTimePassword, OneTimePasswordEntity? entity)
  {
    if (oneTimePassword == null || entity == null)
    {
      Assert.Null(oneTimePassword);
      Assert.Null(entity);
      return;
    }

    Assert.Equal(oneTimePassword.Version, entity.Version);
    Assert.Equal(oneTimePassword.CreatedBy.Value, entity.CreatedBy);
    Assertions.Equal(oneTimePassword.CreatedOn, entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(oneTimePassword.UpdatedBy.Value, entity.UpdatedBy);
    Assertions.Equal(oneTimePassword.UpdatedOn, entity.UpdatedOn, TimeSpan.FromSeconds(1));

    Assert.Equal(oneTimePassword.TenantId?.Value, entity.TenantId);

    Assertions.Equal(oneTimePassword.ExpiresOn, entity.ExpiresOn, TimeSpan.FromSeconds(1));
    Assert.Equal(oneTimePassword.MaximumAttempts, entity.MaximumAttempts);

    Assert.Equal(oneTimePassword.AttemptCount, entity.AttemptCount);
    Assert.Equal(oneTimePassword.HasValidationSucceeded, entity.HasValidationSucceeded);

    Assert.Equal(oneTimePassword.CustomAttributes, entity.CustomAttributes);
  }
}
