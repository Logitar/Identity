namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class BlacklistedTokenEntity
{
  public int BlacklistedTokenId { get; private set; }

  public string TokenId { get; private set; }

  public DateTime? ExpiresOn { get; set; }

  public BlacklistedTokenEntity(string tokenId)
  {
    TokenId = tokenId;
  }

  private BlacklistedTokenEntity() : this(string.Empty)
  {
  }
}
