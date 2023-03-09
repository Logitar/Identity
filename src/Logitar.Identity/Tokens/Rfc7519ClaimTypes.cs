namespace Logitar.Identity.Tokens;

/// <summary>
/// The list of claims defined by the <see href="https://www.rfc-editor.org/rfc/rfc7519">RFC 7519</see> specification used by the identity system.
/// </summary>
public static class Rfc7519ClaimTypes
{
  /// <summary>
  /// The identifier of the token.
  /// </summary>
  public const string JwtId = "jti";
  /// <summary>
  /// The subject of the token.
  /// </summary>
  public const string Subject = "sub";
}
