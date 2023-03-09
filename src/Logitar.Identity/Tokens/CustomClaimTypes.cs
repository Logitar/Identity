namespace Logitar.Identity.Tokens;

/// <summary>
/// The list of custom claims used by the identity system.
/// </summary>
public static class CustomClaimTypes
{
  /// <summary>
  /// The purpose of tokens. This claim ensures a token created for a specified purpose cannot be
  /// used for another purpose.
  /// </summary>
  public const string Purpose = "purpose";
}
