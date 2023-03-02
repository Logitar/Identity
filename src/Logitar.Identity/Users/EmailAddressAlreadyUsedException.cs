using Logitar.Identity.Realms;
using System.Text;

namespace Logitar.Identity;

/// <summary>
/// The exception thrown when an email address is already used.
/// </summary>
public class EmailAddressAlreadyUsedException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EmailAddressAlreadyUsedException"/> class using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm in which the email address is already used.</param>
  /// <param name="emailAddress">The email address that is already used.</param>
  /// <param name="paramName">The name of the parameter associated to the email address.</param>
  public EmailAddressAlreadyUsedException(RealmAggregate realm, string emailAddress, string paramName) : base(GetMessage(realm, emailAddress, paramName))
  {
    Data["Realm"] = realm.ToString();
    Data["EmailAddress"] = emailAddress;
    Data["ParamName"] = paramName;
  }

  /// <summary>
  /// Builds the exception message using the specified arguments.
  /// </summary>
  /// <param name="realm">The realm in which the email address is already used.</param>
  /// <param name="emailAddress">The email address that is already used.</param>
  /// <param name="paramName">The name of the parameter associated to the email address.</param>
  /// <returns>The exception message.</returns>
  private static string GetMessage(RealmAggregate realm, string emailAddress, string paramName)
  {
    StringBuilder message = new();

    message.AppendLine("The specified email address is already used.");
    message.AppendLine($"Realm: {realm}");
    message.AppendLine($"Email address: {emailAddress}");
    message.AppendLine($"ParamName: {paramName}");

    return message.ToString();
  }
}
