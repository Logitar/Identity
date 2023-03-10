using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Tokens;

/// <summary>
/// The exception thrown when a security token is blacklisted.
/// </summary>
public class SecurityTokenBlacklistedException : SecurityTokenValidationException
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SecurityTokenBlacklistedException"/> class using the specified arguments.
  /// </summary>
  /// <param name="blacklistedIds">The list of blacklisted identifiers.</param>
  public SecurityTokenBlacklistedException(IEnumerable<Guid> blacklistedIds) : base(GetMessage(blacklistedIds))
  {
    Data["BlacklistedIds"] = blacklistedIds;
  }

  /// <summary>
  /// Builds the exception message using the specified arguments.
  /// </summary>
  /// <param name="blacklistedIds">The list of blacklisted identifiers.</param>
  /// <returns>The exception message.</returns>
  private static string GetMessage(IEnumerable<Guid> blacklistedIds)
  {
    StringBuilder message = new();

    message.AppendLine("The security token is blacklisted.");
    message.AppendLine("Blacklisted identifiers:");
    foreach (Guid blacklistedId in blacklistedIds)
    {
      message.Append(" - ").Append(blacklistedId).AppendLine();
    }

    return message.ToString();
  }
}
