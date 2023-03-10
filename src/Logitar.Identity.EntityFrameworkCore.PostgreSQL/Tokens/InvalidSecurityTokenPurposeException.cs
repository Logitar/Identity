using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Tokens;

/// <summary>
/// The exception thrown when a security token does not serve the required purpose.
/// </summary>
public class InvalidSecurityTokenPurposeException : SecurityTokenValidationException
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidSecurityTokenPurposeException"/> class using the specified arguments.
  /// </summary>
  /// <param name="requiredPurpose">The required token purpose.</param>
  /// <param name="actualPurposes">The actual token purposes.</param>
  public InvalidSecurityTokenPurposeException(string requiredPurpose, IEnumerable<string> actualPurposes)
    : base(GetMessage(requiredPurpose, actualPurposes))
  {
    Data["RequiredPurpose"] = requiredPurpose;
    Data["ActualPurposes"] = actualPurposes;
  }

  /// <summary>
  /// Builds the exception message using the specified arguments.
  /// </summary>
  /// <param name="requiredPurpose">The required token purpose.</param>
  /// <param name="actualPurposes">The actual token purposes.</param>
  /// <returns>The exception message.</returns>
  private static string GetMessage(string requiredPurpose, IEnumerable<string> actualPurposes)
  {
    StringBuilder message = new();

    message.AppendLine("The security token does not serve the required purpose.");
    message.AppendLine($"Required purpose: {requiredPurpose}");
    message.AppendLine($"Actual purposes: {string.Join(", ", actualPurposes ?? Enumerable.Empty<string>())}");

    return message.ToString();
  }
}
