using Logitar.Identity.Realms;

namespace Logitar.Identity.ApiKeys;

/// <summary>
/// Exposes methods to load API keys from the event store.
/// </summary>
public interface IApiKeyRepository
{
  /// <summary>
  /// Retrieves the list of API keys in the specified realm.
  /// </summary>
  /// <param name="realm">The realm the API keys belong to.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of API keys, or empty if none.</returns>
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RealmAggregate realm, CancellationToken cancellationToken = default);
}
