using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Roles;

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
  Task<RoleAggregate?> LoadAsync(RoleId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<RoleAggregate?> LoadAsync(RoleId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the role if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<RoleAggregate?> LoadAsync(RoleId id, bool includeDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the role.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the role if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<RoleAggregate?> LoadAsync(RoleId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles from the event store.
  /// </summary>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted roles.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<RoleId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted roles.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<RoleId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted roles.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads a role by the specified unique name.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The role, if found.</returns>
  Task<RoleAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the roles of the specified API key.
  /// </summary>
  /// <param name="apiKey">The API key.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the roles of the specified user.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified role into the store.
  /// </summary>
  /// <param name="role">The role to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified roles into the store.
  /// </summary>
  /// <param name="roles">The roles to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken = default);
}
