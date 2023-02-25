using Logitar.Identity.Realms;
using System.Text;

namespace Logitar.Identity;

/// <summary>
/// The exception thrown when an username is already used.
/// </summary>
public class UsernameAlreadyUsedException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UsernameAlreadyUsedException"/> class using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm in which the username is already used.</param>
  /// <param name="username">The username that is already used.</param>
  /// <param name="paramName">The name of the parameter associated to the username.</param>
  public UsernameAlreadyUsedException(RealmAggregate realm, string username, string paramName) : base(GetMessage(realm, username, paramName))
  {
    Data["Realm"] = realm.ToString();
    Data["Username"] = username;
    Data["ParamName"] = paramName;
  }

  /// <summary>
  /// Builds the exception message using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm in which the username is already used.</param>
  /// <param name="username">The username that is already used.</param>
  /// <param name="paramName">The name of the parameter associated to the username.</param>
  /// <returns>The exception message.</returns>
  private static string GetMessage(RealmAggregate realm, string username, string paramName)
  {
    StringBuilder message = new();

    message.AppendLine("The specified username is already used.");
    message.AppendLine($"Realm: {realm}");
    message.AppendLine($"Username: {username}");
    message.AppendLine($"ParamName: {paramName}");

    return message.ToString();
  }
}
