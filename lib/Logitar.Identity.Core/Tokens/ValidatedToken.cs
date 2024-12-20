using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// Represents a validated token.
/// </summary>
public record ValidatedToken
{
  /// <summary>
  /// Gets the validated claims principal.
  /// </summary>
  public ClaimsPrincipal ClaimsPrincipal { get; }
  /// <summary>
  /// Gets the validated security token.
  /// </summary>
  public SecurityToken SecurityToken { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidatedToken"/> class.
  /// </summary>
  /// <param name="claimsPrincipal">The validated claims principal.</param>
  /// <param name="securityToken">The validated security token.</param>
  public ValidatedToken(ClaimsPrincipal claimsPrincipal, SecurityToken securityToken)
  {
    ClaimsPrincipal = claimsPrincipal;
    SecurityToken = securityToken;
  }
}
