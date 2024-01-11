using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserManager : IUserManager
{
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IUserSettingsResolver _userSettingsResolver;

  public UserManager(ISessionRepository sessionRepository, IUserRepository userRepository, IUserSettingsResolver userSettingsResolver)
  {
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
    _userSettingsResolver = userSettingsResolver;
  }

  public async Task<UserAggregate?> FindAsync(string? tenantIdValue, string uniqueNameOrEmailAddress, CancellationToken cancellationToken)
  {
    IUserSettings userSettings = _userSettingsResolver.Resolve();

    TenantId? tenantId = TenantId.TryCreate(tenantIdValue); // TODO(fpion): shouldn't validate
    UniqueNameUnit uniqueName = new(userSettings.UniqueName, uniqueNameOrEmailAddress); // TODO(fpion): shouldn't validate
    UserAggregate? user = await _userRepository.LoadAsync(tenantId, uniqueName, cancellationToken);

    if (user == null && userSettings.RequireUniqueEmail)
    {
      EmailUnit email = new(uniqueNameOrEmailAddress);
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(tenantId, email, cancellationToken);
      if (users.Count() == 1)
      {
        user = users.Single();
      }
    }

    return user;
  }

  public async Task SaveAsync(UserAggregate user, ActorId actorId, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool hasEmailChanged = false;
    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in user.Changes)
    {
      if (change is UserCreatedEvent || change is UserUniqueNameChangedEvent)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is UserEmailChangedEvent)
      {
        hasEmailChanged = true;
      }
      else if (change is UserDeletedEvent)
      {
        hasBeenDeleted = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, user.UniqueName);
      }
    }

    IUserSettings userSettings = _userSettingsResolver.Resolve();
    if (hasEmailChanged && user.Email != null && userSettings.RequireUniqueEmail)
    {
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
      if (users.Any(other => !other.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, user.Email);
      }
    }

    foreach (DomainEvent change in user.Changes)
    {
      if (change is UserIdentifierChangedEvent identifier)
      {
        UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, identifier.Key, identifier.Value, cancellationToken);
        if (other?.Equals(user) == false)
        {
          throw new CustomIdentifierAlreadyUsedException<UserAggregate>(user.TenantId, identifier.Key, identifier.Value);
        }
      }
    }

    if (hasBeenDeleted)
    {
      IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(user, cancellationToken);
      foreach (SessionAggregate session in sessions)
      {
        session.Delete(actorId);
      }
      await _sessionRepository.SaveAsync(sessions, cancellationToken);
    }

    await _userRepository.SaveAsync(user, cancellationToken);
  }
}
