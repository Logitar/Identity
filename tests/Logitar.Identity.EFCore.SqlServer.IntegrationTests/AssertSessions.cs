using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

internal static class AssertSessions
{
  public static void AreEqual(SessionAggregate? session, SessionEntity? entity)
  {
    if (session == null || entity == null)
    {
      Assert.Null(session);
      Assert.Null(entity);
      return;
    }

    Assert.Equal(session.Version, entity.Version);
    Assert.Equal(session.CreatedBy.Value, entity.CreatedBy);
    Assertions.Equal(session.CreatedOn, entity.CreatedOn, TimeSpan.FromMinutes(1));
    Assert.Equal(session.UpdatedBy.Value, entity.UpdatedBy);
    Assertions.Equal(session.UpdatedOn, entity.UpdatedOn, TimeSpan.FromMinutes(1));

    Assert.NotNull(entity.User);
    Assert.Equal(session.UserId.Value, entity.User.AggregateId);

    if (session.IsPersistent)
    {
      Assert.True(entity.IsPersistent);
      Assert.NotNull(entity.SecretHash);
    }
    else
    {
      Assert.False(entity.IsPersistent);
      Assert.Null(entity.SecretHash);
    }

    if (session.IsActive)
    {
      Assert.Null(entity.SignedOutBy);
      Assert.Null(entity.SignedOutOn);
      Assert.True(entity.IsActive);
    }
    else
    {
      Assert.NotNull(entity.SignedOutBy);
      Assert.NotNull(entity.SignedOutOn);
      Assert.False(entity.IsActive);
    }

    Assert.Equal(session.CustomAttributes, entity.CustomAttributes);
  }
}
