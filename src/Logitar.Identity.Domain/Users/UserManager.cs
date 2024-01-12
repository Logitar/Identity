using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Implements methods to manage users.
/// </summary>
public class UserManager : IUserManager
{
  /// <summary>
  /// Gets the session repository.
  /// </summary>
  protected ISessionRepository SessionRepository { get; }
  /// <summary>
  /// Gets the user repository.
  /// </summary>
  protected IUserRepository UserRepository { get; }
  /// <summary>
  /// Gets the user settings resolver.
  /// </summary>
  protected IUserSettingsResolver UserSettingsResolver { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserManager"/> class.
  /// </summary>
  /// <param name="sessionRepository">The session repository.</param>
  /// <param name="userRepository">The user repository.</param>
  /// <param name="userSettingsResolver">The user settings resolver.</param>
  public UserManager(ISessionRepository sessionRepository, IUserRepository userRepository, IUserSettingsResolver userSettingsResolver)
  {
    SessionRepository = sessionRepository;
    UserRepository = userRepository;
    UserSettingsResolver = userSettingsResolver;
  }

  /// <summary>
  /// Saves the specified user, performing model validation such as unique name and email address unicity.
  /// </summary>
  /// <param name="user">The user to save.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task SaveAsync(UserAggregate user, ActorId actorId, CancellationToken cancellationToken)
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
      UserAggregate? other = await UserRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, user.UniqueName);
      }
    }

    IUserSettings userSettings = UserSettingsResolver.Resolve();
    if (hasEmailChanged && user.Email != null && userSettings.RequireUniqueEmail)
    {
      IEnumerable<UserAggregate> users = await UserRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
      if (users.Any(other => !other.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, user.Email);
      }
    }

    foreach (DomainEvent change in user.Changes)
    {
      if (change is UserIdentifierChangedEvent identifier)
      {
        UserAggregate? other = await UserRepository.LoadAsync(user.TenantId, identifier.Key, identifier.Value, cancellationToken);
        if (other?.Equals(user) == false)
        {
          throw new CustomIdentifierAlreadyUsedException<UserAggregate>(user.TenantId, identifier.Key, identifier.Value);
        }
      }
    }

    if (hasBeenDeleted)
    {
      IEnumerable<SessionAggregate> sessions = await SessionRepository.LoadAsync(user, cancellationToken);
      foreach (SessionAggregate session in sessions)
      {
        session.Delete(actorId);
      }
      await SessionRepository.SaveAsync(sessions, cancellationToken);
    }

    await UserRepository.SaveAsync(user, cancellationToken);
  }
}
