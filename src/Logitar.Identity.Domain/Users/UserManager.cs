using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts.Settings;
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
  /// Tries finding an user by its unique identifier, unique name, or email address if they are unique.
  /// </summary>
  /// <param name="tenantIdValue">The identifier of the tenant in which to search.</param>
  /// <param name="id">The identifier of the user to find.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  public virtual async Task<FoundUsers> FindAsync(string? tenantIdValue, string id, CancellationToken cancellationToken)
  {
    return await FindAsync(tenantIdValue, id, userSettings: null, cancellationToken);
  }
  /// <summary>
  /// Tries finding an user by its unique identifier, unique name, or email address if they are unique.
  /// </summary>
  /// <param name="tenantIdValue">The identifier of the tenant in which to search.</param>
  /// <param name="id">The identifier of the user to find.</param>
  /// <param name="userSettings">The user settings.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  public virtual async Task<FoundUsers> FindAsync(string? tenantIdValue, string id, IUserSettings? userSettings, CancellationToken cancellationToken)
  {
    userSettings ??= UserSettingsResolver.Resolve();

    TenantId? tenantId = null;
    try
    {
      tenantId = TenantId.TryCreate(tenantIdValue);
    }
    catch (ValidationException)
    {
    }

    UserId? userId = null;
    try
    {
      userId = UserId.TryCreate(id);
    }
    catch (ValidationException)
    {
    }

    UniqueNameUnit? uniqueName = null;
    try
    {
      uniqueName = new(userSettings.UniqueName, id);
    }
    catch (ValidationException)
    {
    }

    EmailUnit? email = null;
    try
    {
      email = new(id);
    }
    catch (ValidationException)
    {
    }

    FoundUsers found = new();

    if (userId != null)
    {
      found.ById = await UserRepository.LoadAsync(userId, cancellationToken);
    }
    if (uniqueName != null)
    {
      found.ByUniqueName = await UserRepository.LoadAsync(tenantId, uniqueName, cancellationToken);
    }
    if (email != null && userSettings.RequireUniqueEmail)
    {
      IEnumerable<UserAggregate> users = await UserRepository.LoadAsync(tenantId, email, cancellationToken);
      if (users.Count() == 1)
      {
        found.ByEmail = users.Single();
      }
    }

    return found;
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
    await SaveAsync(user, userSettings: null, actorId, cancellationToken);
  }
  /// <summary>
  /// Saves the specified user, performing model validation such as unique name and email address unicity.
  /// </summary>
  /// <param name="user">The user to save.</param>
  /// <param name="userSettings">The user settings.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task SaveAsync(UserAggregate user, IUserSettings? userSettings, ActorId actorId, CancellationToken cancellationToken)
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

    if (hasEmailChanged && user.Email != null)
    {
      userSettings ??= UserSettingsResolver.Resolve();
      if (userSettings.RequireUniqueEmail)
      {
        IEnumerable<UserAggregate> users = await UserRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
        if (users.Any(other => !other.Equals(user)))
        {
          throw new EmailAddressAlreadyUsedException(user.TenantId, user.Email);
        }
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
