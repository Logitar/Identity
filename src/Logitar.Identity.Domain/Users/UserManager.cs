using Logitar.EventSourcing;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserManager : IUserManager
{
  public UserManager(IUserRepository userRepository, IUserSettings userSettings) // TODO(fpion): not multitenant-compatible
  {
    UserRepository = userRepository;
    UserSettings = userSettings;
  }

  protected IUserRepository UserRepository { get; }
  protected IUserSettings UserSettings { get; }

  public virtual async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool emailHasChanged = false;
    bool uniqueNameHasChanged = false;
    foreach (DomainEvent change in user.Changes)
    {
      if (change is UserCreatedEvent || change is UserUniqueNameChangedEvent)
      {
        uniqueNameHasChanged = true;
      }
      else if (change is UserDeletedEvent)
      {
        hasBeenDeleted = true;
      }
      else if (change is UserEmailChangedEvent)
      {
        emailHasChanged = true;
      }
    }

    if (uniqueNameHasChanged)
    {
      UserAggregate? other = await UserRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, user.UniqueName);
      }
    }

    if (emailHasChanged && user.Email != null && UserSettings.RequireUniqueEmail)
    {
      IEnumerable<UserAggregate> others = await UserRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
      if (others.Any(other => !other.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, user.Email);
      }
    }

    if (hasBeenDeleted)
    {
      // TODO(fpion): delete sessions
    }

    await UserRepository.SaveAsync(user, cancellationToken);
  }
}
