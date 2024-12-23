using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Defines methods to retrieve and store roles to an event store.
/// </summary>
public interface IRoleRepository
{
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<Role?> LoadAsync(RoleId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<Role?> LoadAsync(RoleId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="isDeleted">A value indicating whether or not to load the role if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<Role?> LoadAsync(RoleId id, bool? isDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the role.</param>
  /// <param name="isDeleted">A value indicating whether or not to load the role if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<Role?> LoadAsync(RoleId id, long? version, bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles from the event store.
  /// </summary>
  /// <param name="isDeleted">A value indicating whether or not to load deleted roles.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(IEnumerable<RoleId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="isDeleted">A value indicating whether or not to load deleted roles.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(IEnumerable<RoleId> ids, bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads a role by the specified unique name.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<Role?> LoadAsync(TenantId? tenantId, UniqueName uniqueName, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles of the specified API key.
  /// </summary>
  /// <param name="apiKey">The API key.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(ApiKey apiKey, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles of the specified user.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IReadOnlyCollection<Role>> LoadAsync(User user, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified role into the store.
  /// </summary>
  /// <param name="role">The role to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(Role role, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified roles into the store.
  /// </summary>
  /// <param name="roles">The roles to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<Role> roles, CancellationToken cancellationToken = default);
}
