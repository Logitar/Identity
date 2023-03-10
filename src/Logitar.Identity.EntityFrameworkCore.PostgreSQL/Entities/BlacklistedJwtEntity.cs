namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing a blacklisted JSON Web Token.
/// </summary>
internal class BlacklistedJwtEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BlacklistedJwtEntity"/> class using the specified arguments.
  /// </summary>
  /// <param name="id">The identifier to blacklist.</param>
  /// <param name="expiresOn">The date and time when the blacklisting will expire.</param>
  public BlacklistedJwtEntity(Guid id, DateTime? expiresOn = null)
  {
    Id = id;
    ExpiresOn = expiresOn;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="BlacklistedJwtEntity"/> class.
  /// </summary>
  private BlacklistedJwtEntity()
  {
  }

  /// <summary>
  /// Gets or sets the identifier of the blacklisting.
  /// </summary>
  public long BlacklistedJwtId { get; private set; }

  /// <summary>
  /// Gets or sets the blacklisted identifier.
  /// </summary>
  public Guid Id { get; private set; }

  /// <summary>
  /// Gets or sets the date and time when the blacklisting will expire.
  /// </summary>
  public DateTime? ExpiresOn { get; private set; }
}
