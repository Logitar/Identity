using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using Logitar.Identity.Users.Validators;

namespace Logitar.Identity.Users;

/// <summary>
/// Implements methods to help managing users.
/// </summary>
internal class UserHelper : IUserHelper
{
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserHelper"/> class using the specified arguments.
  /// </summary>
  /// <param name="roleRepository">The role repository.</param>
  public UserHelper(IRoleRepository roleRepository)
  {
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="input">The user creation input data.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  public async Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    CreateUserInput input,
    CancellationToken cancellationToken = default)
  {
    return await GetRolesAsync(realm, input.Roles, nameof(input.Roles), cancellationToken);
  }

  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="input">The user update input data.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  public async Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    UpdateUserInput input,
    CancellationToken cancellationToken = default)
  {
    return await GetRolesAsync(realm, input.Roles, nameof(input.Roles), cancellationToken);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified password matches the specified user.
  /// </summary>
  /// <param name="user">The user to compare.</param>
  /// <param name="password">The password to match.</param>
  /// <returns>True if the password matches the user's salted and hashed password.</returns>
  public bool IsMatch(UserAggregate user, string password)
  {
    if (!user.HasPassword)
    {
      return false;
    }

    Pbkdf2 pbkdf2 = Pbkdf2.Parse(user.PasswordHash!);

    return pbkdf2.IsMatch(password);
  }

  /// <summary>
  /// Validates the specified password in the specified realm. If the password matches the realm
  /// password constraints, a salted and hashed password will be returned. If the password does not
  /// match the realm password constraints, a <see cref="ValidationException"/> will be thrown.
  /// </summary>
  /// <param name="realm">The realm defining password constraints.</param>
  /// <param name="password">The password to validate, salt and hash.</param>
  /// <returns>The salted and hashed password.</returns>
  public string ValidateAndHashPassword(RealmAggregate realm, string password)
  {
    PasswordValidator validator = new(realm.PasswordSettings);
    validator.ValidateAndThrow(password);

    return new Pbkdf2(password).ToString();
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
