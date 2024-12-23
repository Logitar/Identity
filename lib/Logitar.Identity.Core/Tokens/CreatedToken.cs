using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// Represents a created token.
/// </summary>
public record CreatedToken
{
  /// <summary>
  /// Gets the created security token.
  /// </summary>
  public SecurityToken SecurityToken { get; }
  /// <summary>
  /// Gets a string representation of the created token.
  /// </summary>
  public string TokenString { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="CreatedToken"/> class.
  /// </summary>
  /// <param name="securityToken">The created security token.</param>
  /// <param name="tokenString">A string representation of the created token.</param>
  public CreatedToken(SecurityToken securityToken, string tokenString)
  {
    SecurityToken = securityToken;
    TokenString = tokenString;
  }
}
