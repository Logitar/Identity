using Logitar.EventSourcing;

namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// Represents the identifier of an API key.
/// </summary>
public readonly struct ApiKeyId
{
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
  /// Initializes a new instance of the <see cref="ApiKeyId"/> struct.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <param name="entityId">The entity identifier.</param>
  public ApiKeyId(TenantId? tenantId, Guid entityId) : this(tenantId, Convert.ToBase64String(entityId.ToByteArray()).ToUriSafeBase64())
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyId"/> struct.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <param name="entityId">The entity identifier.</param>
  public ApiKeyId(TenantId? tenantId, string entityId)
  {
    TenantId = tenantId;
    EntityId = new(entityId);
    StreamId = new(tenantId.HasValue ? $"{tenantId}:{entityId}" : entityId);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyId"/> struct.
  /// </summary>
  /// <param name="streamId">A stream identifier.</param>
  public ApiKeyId(StreamId streamId)
  {
    StreamId = streamId;
  }

  /// <summary>
  /// Randomly generates a new API key identifier.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <returns>The generated identifier.</returns>
  public static ApiKeyId NewId(TenantId? tenantId = null) => new(tenantId, Guid.NewGuid());

  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are equal.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are equal.</returns>
  public static bool operator ==(ApiKeyId left, ApiKeyId right) => left.Equals(right);
  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are different.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are different.</returns>
  public static bool operator !=(ApiKeyId left, ApiKeyId right) => !left.Equals(right);

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the identifier.
  /// </summary>
  /// <param name="obj">The object to be compared to.</param>
  /// <returns>True if the object is equal to the identifier.</returns>
  public override bool Equals([NotNullWhen(true)] object? obj) => obj is ApiKeyId id && id.Value == Value;
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
