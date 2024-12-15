using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Domain.Tokens;

/// <summary>
/// The exception raised when a validated security token is blacklisted.
/// </summary>
public class SecurityTokenBlacklistedException : SecurityTokenValidationException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public const string ErrorMessage = "The security token is blacklisted.";

  /// <summary>
  /// Gets or sets the list of blacklisted token identifiers.
  /// </summary>
  public IEnumerable<string> BlacklistedIds
  {
    get => (IEnumerable<string>)Data[nameof(BlacklistedIds)]!;
    private set => Data[nameof(BlacklistedIds)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SecurityTokenBlacklistedException"/> class.
  /// </summary>
  /// <param name="blacklistedIds">The list of the blacklisted token identifiers.</param>
  public SecurityTokenBlacklistedException(IEnumerable<string> blacklistedIds) : base(BuildMessage(blacklistedIds))
  {
    BlacklistedIds = blacklistedIds;
  }

  private static string BuildMessage(IEnumerable<string> blacklistedIds)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.AppendLine("BlacklistedIds:");
    foreach (string blacklistedId in blacklistedIds)
    {
      message.Append(" - ").Append(blacklistedId).AppendLine();
    }

    return message.ToString();
  }
}
