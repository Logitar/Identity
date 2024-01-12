using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Defines methods to retrieve and store users to an event store.
/// </summary>
public interface IUserRepository
{
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<UserAggregate?> LoadAsync(UserId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<UserAggregate?> LoadAsync(UserId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the user if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<UserAggregate?> LoadAsync(UserId id, bool includeDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the user.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the user if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<UserAggregate?> LoadAsync(UserId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users from the event store.
  /// </summary>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted users.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted users.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<UserId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted users.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads an user by the specified unique name.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<UserAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users by the specified email address.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="email">The email address.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(TenantId? tenantId, EmailUnit email, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified custom identifier.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="identifierKey">The key of the custom identifier.</param>
  /// <param name="identifierValue">The value of the custom identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<UserAggregate?> LoadAsync(TenantId? tenantId, string identifierKey, string identifierValue, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users having the specified role.
  /// </summary>
  /// <param name="role">The role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the user of the specified session.
  /// </summary>
  /// <param name="session">The session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<UserAggregate> LoadAsync(SessionAggregate session, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified user into the store.
  /// </summary>
  /// <param name="user">The user to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified users into the store.
  /// </summary>
  /// <param name="users">The users to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken = default);
}
