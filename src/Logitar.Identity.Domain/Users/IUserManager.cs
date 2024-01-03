namespace Logitar.Identity.Domain.Users;

public interface IUserManager
{
  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
}
