using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Domain.Tokens;

[Trait(Traits.Category, Categories.Unit)]
public class ValidatedTokenTests
{
  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  [Fact(DisplayName = "ctor: it should construct the correct instance.")]
  public void ctor_it_should_construct_the_correct_instance()
  {
    ClaimsPrincipal claimsPrincipal = new();
    SecurityTokenDescriptor tokenDescriptor = new();
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);

    ValidatedToken validatedToken = new(claimsPrincipal, securityToken);
    Assert.Same(claimsPrincipal, validatedToken.ClaimsPrincipal);
    Assert.Same(securityToken, validatedToken.SecurityToken);
  }
}
