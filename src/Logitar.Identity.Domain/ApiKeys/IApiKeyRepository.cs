using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

/// <summary>
/// Defines methods to retrieve and store API keys to an event store.
/// </summary>
public interface IApiKeyRepository
{
  /// <summary>
  /// Loads an API key by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API key, if found.</returns>
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an API key by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the API key.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API key, if found.</returns>
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an API key by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the API key if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API key, if found.</returns>
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, bool includeDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads an API key by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the API key.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the API key if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API key, if found.</returns>
  Task<ApiKeyAggregate?> LoadAsync(ApiKeyId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the API keys from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the API keys from the event store.
  /// </summary>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted API keys.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the API keys by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(IEnumerable<ApiKeyId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the API keys by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted API keys.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(IEnumerable<ApiKeyId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the API keys in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the API keys in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted API keys.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the API keys having the specified role.
  /// </summary>
  /// <param name="role">The role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found API keys.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified API key into the store.
  /// </summary>
  /// <param name="apiKey">The API key to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified API keys into the store.
  /// </summary>
  /// <param name="apiKeys">The API keys to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken = default);
}
