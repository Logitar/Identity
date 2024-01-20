using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Domain.Tokens;

[Trait(Traits.Category, Categories.Unit)]
public class CreatedTokenTests
{
  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  [Fact(DisplayName = "ctor: it should construct the correct instance.")]
  public void ctor_it_should_construct_the_correct_instance()
  {
    SecurityTokenDescriptor tokenDescriptor = new();
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string tokenString = _tokenHandler.WriteToken(securityToken);

    CreatedToken createdToken = new(securityToken, tokenString);
    Assert.Same(securityToken, createdToken.SecurityToken);
    Assert.Equal(tokenString, createdToken.TokenString);
  }
}
