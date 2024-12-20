using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;

namespace Logitar.Identity.Core.Users;

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
  Task<User?> LoadAsync(UserId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<User?> LoadAsync(UserId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="isDeleted">A value indicating whether or not to load the user if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<User?> LoadAsync(UserId id, bool? isDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the user.</param>
  /// <param name="isDeleted">A value indicating whether or not to load the user if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<User?> LoadAsync(UserId id, long? version, bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users from the event store.
  /// </summary>
  /// <param name="isDeleted">A value indicating whether or not to load deleted users.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="isDeleted">A value indicating whether or not to load deleted users.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(IEnumerable<UserId> ids, bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads an user by the specified unique name.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<User?> LoadAsync(TenantId? tenantId, UniqueName uniqueName, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the users by the specified email address.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="email">The email address.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(TenantId? tenantId, Email email, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an user by the specified custom identifier.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="identifierKey">The key of the custom identifier.</param>
  /// <param name="identifierValue">The value of the custom identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user, if found.</returns>
  Task<User?> LoadAsync(TenantId? tenantId, Identifier identifierKey, CustomIdentifier identifierValue, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the users having the specified role.
  /// </summary>
  /// <param name="role">The role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<IReadOnlyCollection<User>> LoadAsync(Role role, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the user of the specified session.
  /// </summary>
  /// <param name="session">The session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found users.</returns>
  Task<User> LoadAsync(Session session, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified user into the store.
  /// </summary>
  /// <param name="user">The user to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(User user, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified users into the store.
  /// </summary>
  /// <param name="users">The users to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<User> users, CancellationToken cancellationToken = default);
}
