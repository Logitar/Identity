using Logitar.EventSourcing;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserManager : IUserManager
{
  private readonly IUserRepository _userRepository;
  private readonly IUserSettings _userSettings;

  public UserManager(IUserRepository userRepository, IUserSettings userSettings) // TODO(fpion): not multitenant-compatible
  {
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

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
      UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, user.UniqueName);
      }
    }

    if (emailHasChanged && user.Email != null && _userSettings.RequireUniqueEmail)
    {
      IEnumerable<UserAggregate> others = await _userRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
      if (others.Any(other => !other.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, user.Email);
      }
    }

    if (hasBeenDeleted)
    {
      // TODO(fpion): delete sessions
    }

    await _userRepository.SaveAsync(user, cancellationToken);
  }
}
