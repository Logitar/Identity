using FluentValidation;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;

namespace Logitar.Identity.Users;

/// <summary>
/// Defines methods to help managing users.
/// </summary>
internal interface IUserHelper
{
  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="input">The user creation input data.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    CreateUserInput input,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Resolves a list of roles using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm that the roles should belong to.</param>
  /// <param name="input">The user update input data.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of the roles.</returns>
  Task<IEnumerable<RoleAggregate>?> GetRolesAsync(RealmAggregate realm,
    UpdateUserInput input,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns a value indicating whether or not the specified password matches the specified user.
  /// </summary>
  /// <param name="user">The user to compare.</param>
  /// <param name="password">The password to match.</param>
  /// <returns>True if the password matches the user's salted and hashed password.</returns>
  bool IsMatch(UserAggregate user, string password);

  /// <summary>
  /// Validates the specified password in the specified realm. If the password matches the realm
  /// password constraints, a salted and hashed password will be returned. If the password does not
  /// match the realm password constraints, a <see cref="ValidationException"/> will be thrown.
  /// </summary>
  /// <param name="realm">The realm defining password constraints.</param>
  /// <param name="password">The password to validate, salt and hash.</param>
  /// <returns>The salted and hashed password.</returns>
  string ValidateAndHashPassword(RealmAggregate realm, string password);
}
