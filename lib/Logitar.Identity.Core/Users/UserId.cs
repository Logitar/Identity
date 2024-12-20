using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents the identifier of an user.
/// </summary>
public readonly struct UserId
{
  /// <summary>
  /// The separator between the tenant ID and the entity ID.
  /// </summary>
  private const char Separator = ':';

  /// <summary>
  /// Gets the identifier of the event stream.
  /// </summary>
  public StreamId StreamId { get; }
  /// <summary>
  /// Gets the value of the identifier.
  /// </summary>
  public string Value => StreamId.Value;

  /// <summary>
  /// Gets the tenant identifier.
  /// </summary>
  public TenantId? TenantId { get; }
  /// <summary>
  /// Gets the entity identifier.
  /// </summary>
  public EntityId EntityId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserId"/> struct.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <param name="entityId">The entity identifier.</param>
  public UserId(TenantId? tenantId, Guid entityId) : this(tenantId, Convert.ToBase64String(entityId.ToByteArray()).ToUriSafeBase64())
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="UserId"/> struct.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <param name="entityId">The entity identifier.</param>
  public UserId(TenantId? tenantId, string entityId)
  {
    TenantId = tenantId;
    EntityId = new(entityId);
    StreamId = new(tenantId.HasValue ? string.Join(Separator, tenantId, entityId) : entityId);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="UserId"/> struct.
  /// </summary>
  /// <param name="streamId">A stream identifier.</param>
  public UserId(StreamId streamId)
  {
    StreamId = streamId;

    string[] values = streamId.Value.Split(Separator);
    TenantId = values.Length == 2 ? new(values[0]) : null;
    EntityId = new(values.Last());
  }

  /// <summary>
  /// Randomly generates a new user identifier.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <returns>The generated identifier.</returns>
  public static UserId NewId(TenantId? tenantId = null) => new(tenantId, Guid.NewGuid());

  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are equal.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are equal.</returns>
  public static bool operator ==(UserId left, UserId right) => left.Equals(right);
  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are different.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are different.</returns>
  public static bool operator !=(UserId left, UserId right) => !left.Equals(right);

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the identifier.
  /// </summary>
  /// <param name="obj">The object to be compared to.</param>
  /// <returns>True if the object is equal to the identifier.</returns>
  public override bool Equals([NotNullWhen(true)] object? obj) => obj is UserId id && id.Value == Value;
  /// <summary>
  /// Returns the hash code of the current identifier.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode() => Value.GetHashCode();
  /// <summary>
  /// Returns a string representation of the identifier.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;
}
