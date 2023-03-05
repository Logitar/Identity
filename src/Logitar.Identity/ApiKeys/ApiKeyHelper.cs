using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using System.Security.Cryptography;

namespace Logitar.Identity.ApiKeys;

/// <summary>
/// Implements methods to help managing API keys.
/// </summary>
internal class ApiKeyHelper : IApiKeyHelper
{
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyHelper"/> class using the specified arguments.
  /// </summary>
  /// <param name="roleRepository">The role repository.</param>
  public ApiKeyHelper(IRoleRepository roleRepository)
  {
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Generates an API key secret.
  /// </summary>
  /// <param name="secret">The secret bytes.</param>
  /// <returns>The salted and hashed secret.</returns>
  public string GenerateSecret(out byte[] secret)
  {
    secret = RandomNumberGenerator.GetBytes(32);

    return new Pbkdf2(Convert.ToBase64String(secret)).ToString();
  }

  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="input">The API key creation input data.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  public async Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    CreateApiKeyInput input,
    CancellationToken cancellationToken = default)
  {
    return await GetRolesAsync(realm, input.Roles, nameof(input.Roles), cancellationToken);
  }

  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="input">The API key update input data.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  public async Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    UpdateApiKeyInput input,
    CancellationToken cancellationToken = default)
  {
    return await GetRolesAsync(realm, input.Roles, nameof(input.Roles), cancellationToken);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified secret matches the specified API key.
  /// </summary>
  /// <param name="apiKey">The API key to compare.</param>
  /// <param name="secret">The secret to match.</param>
  /// <returns>True if the secret matches the API key's salted and hashed secret.</returns>
  public bool IsMatch(ApiKeyAggregate apiKey, byte[] secret)
  {
    Pbkdf2 pbkdf2 = Pbkdf2.Parse(apiKey.SecretHash);

    return pbkdf2.IsMatch(Convert.ToBase64String(secret));
  }

  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="ids">The list of role identifiers and unique names.</param>
  /// <param name="paramName">The name of the role list parameter.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  /// <exception cref="AggregatesNotFoundException{RoleAggregate}">At least one role could not be found.</exception>
  private async Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    IEnumerable<string>? ids,
    string paramName,
    CancellationToken cancellationToken)
  {
    if (ids == null)
    {
      return null;
    }

    List<AggregateId> missingRoles = new(capacity: ids.Count());

    IEnumerable<RoleAggregate> realmRoles = await _roleRepository.LoadAsync(realm, cancellationToken);
    Dictionary<AggregateId, RoleAggregate> rolesById = realmRoles.ToDictionary(x => x.Id, x => x);
    Dictionary<string, RoleAggregate> rolesByUniqueName = realmRoles.ToDictionary(x => x.UniqueName.ToUpper(), x => x);
    List<RoleAggregate> roles = new(capacity: missingRoles.Count);

    foreach (string id in ids)
    {
      AggregateId aggregateId = Guid.TryParse(id, out Guid roleId)
        ? new AggregateId(roleId)
        : new AggregateId(id);

      if (!rolesById.TryGetValue(aggregateId, out RoleAggregate? role) || role == null)
      {
        rolesByUniqueName.TryGetValue(id.ToUpper(), out role);
      }

      if (!roles.AddIfNotNull(role))
      {
        missingRoles.Add(aggregateId);
      }
    }

    if (missingRoles.Any())
    {
      throw new AggregatesNotFoundException<RoleAggregate>(missingRoles, paramName);
    }

    return realmRoles;
  }
}
