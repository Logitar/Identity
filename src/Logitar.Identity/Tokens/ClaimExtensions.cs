using Logitar.Identity.Realms;

namespace Logitar.Identity.Tokens;

/// <summary>
/// Provides extension methods to security claims.
/// </summary>
public static class ClaimExtensions
{
  /// <summary>
  /// Returns the audience claim of the specified realm.
  /// </summary>
  /// <param name="realm">The realm.</param>
  /// <returns>The audience claim.</returns>
  public static string GetAudience(this RealmAggregate realm) => (realm.Url ?? realm.UniqueName).ToLower();

  /// <summary>
  /// Gets the date and time value from the specified security claim.
  /// </summary>
  /// <param name="claim">The security claim.</param>
  /// <returns>The date and time value.</returns>
  public static DateTime GetDateTime(this System.Security.Claims.Claim claim)
    => DateTimeOffset.FromUnixTimeSeconds(long.Parse(claim.Value)).UtcDateTime;

  /// <summary>
  /// Formats the specified audience or issuer string value using the properties in the specified realm.
  /// </summary>
  /// <param name="value">The string to format.</param>
  /// <param name="realm">The realm to use.</param>
  /// <returns>The formatted string value.</returns>
  public static string Format(this string value, RealmAggregate? realm)
    => (realm == null ? value : value.Replace("{UNIQUE_NAME}", realm.UniqueName).Replace("{URL}", realm.Url)).ToLower();

  /// <summary>
  /// Returns the isuer claim of the specified realm.
  /// </summary>
  /// <param name="realm">The realm.</param>
  /// <returns>The issuer claim.</returns>
  public static string GetIssuer(this RealmAggregate realm) => (realm.Url ?? realm.UniqueName).ToLower();
}
