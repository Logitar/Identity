using Logitar.EventSourcing;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Events;

namespace Logitar.Identity.Core.Users;

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
    if (!string.IsNullOrWhiteSpace(tenantIdValue))
    {
      try
      {
        tenantId = new(tenantIdValue);
      }
      catch (Exception)
      {
      }
    }

    UserId? userId = null;
    try
    {
      EntityId entityId = new(id);
      userId = new(tenantId, entityId);
    }
    catch (Exception)
    {
    }

    UniqueName? uniqueName = null;
    try
    {
      UniqueNameSettings uniqueNameSettings = new()
      {
        AllowedCharacters = null
      };
      uniqueName = new(uniqueNameSettings, id);
    }
    catch (Exception)
    {
    }

    Email? email = null;
    try
    {
      email = new(id);
    }
    catch (Exception)
    {
    }

    FoundUsers found = new();

    if (userId.HasValue)
    {
      found.ById = await UserRepository.LoadAsync(userId.Value, cancellationToken);
    }
    if (uniqueName != null)
    {
      found.ByUniqueName = await UserRepository.LoadAsync(tenantId, uniqueName, cancellationToken);
    }
    if (email != null && userSettings.RequireUniqueEmail)
    {
      IReadOnlyCollection<User> users = await UserRepository.LoadAsync(tenantId, email, cancellationToken);
      if (users.Count == 1)
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
  public virtual async Task SaveAsync(User user, ActorId? actorId, CancellationToken cancellationToken)
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
  public virtual async Task SaveAsync(User user, IUserSettings? userSettings, ActorId? actorId, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool hasEmailChanged = false;
    bool hasUniqueNameChanged = false;
    foreach (IEvent change in user.Changes)
    {
      if (change is UserCreated || change is UserUniqueNameChanged)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is UserEmailChanged)
      {
        hasEmailChanged = true;
      }
      else if (change is UserDeleted)
      {
        hasBeenDeleted = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      User? conflict = await UserRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (conflict != null && !conflict.Equals(user))
      {
        throw new UniqueNameAlreadyUsedException(user, conflict);
      }
    }

    if (hasEmailChanged && user.Email != null)
    {
      userSettings ??= UserSettingsResolver.Resolve();
      if (userSettings.RequireUniqueEmail)
      {
        IReadOnlyCollection<User> users = await UserRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
        User? conflict = users.FirstOrDefault(other => !other.Equals(user));
        if (conflict != null)
        {
          throw new EmailAddressAlreadyUsedException(user, conflict);
        }
      }
    }

    foreach (IEvent change in user.Changes)
    {
      if (change is UserIdentifierChanged identifier)
      {
        User? conflict = await UserRepository.LoadAsync(user.TenantId, identifier.Key, identifier.Value, cancellationToken);
        if (conflict != null && !conflict.Equals(user))
        {
          throw new CustomIdentifierAlreadyUsedException(user, conflict, identifier.Key, identifier.Value);
        }
      }
    }

    if (hasBeenDeleted)
    {
      IEnumerable<Session> sessions = await SessionRepository.LoadAsync(user, cancellationToken);
      foreach (Session session in sessions)
      {
        session.Delete(actorId);
      }
      await SessionRepository.SaveAsync(sessions, cancellationToken);
    }

    await UserRepository.SaveAsync(user, cancellationToken);
  }
}
