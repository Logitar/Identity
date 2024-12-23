using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Represents the identifier of a role.
/// </summary>
public readonly struct RoleId
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
  /// Initializes a new instance of the <see cref="RoleId"/> struct.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <param name="entityId">The entity identifier.</param>
  public RoleId(TenantId? tenantId, EntityId entityId)
  {
    StreamId = new(tenantId == null ? entityId.Value : string.Join(Separator, tenantId, entityId));
    TenantId = tenantId;
    EntityId = entityId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleId"/> struct.
  /// </summary>
  /// <param name="streamId">The identifier of the event stream.</param>
  public RoleId(StreamId streamId)
  {
    StreamId = streamId;

    string[] values = streamId.Value.Split(Separator);
    if (values.Length > 2)
    {
      throw new ArgumentException($"The value '{streamId}' is not a valid role ID.", nameof(streamId));
    }
    else if (values.Length == 2)
    {
      TenantId = new(values.First());
    }
    EntityId = new(values.Last());
  }

  /// <summary>
  /// Randomly generates a new role identifier.
  /// </summary>
  /// <param name="tenantId">The tenant identifier.</param>
  /// <returns>The generated identifier.</returns>
  public static RoleId NewId(TenantId? tenantId = null) => new(tenantId, EntityId.NewId());

  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are equal.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are equal.</returns>
  public static bool operator ==(RoleId left, RoleId right) => left.Equals(right);
  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are different.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are different.</returns>
  public static bool operator !=(RoleId left, RoleId right) => !left.Equals(right);

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the identifier.
  /// </summary>
  /// <param name="obj">The object to be compared to.</param>
  /// <returns>True if the object is equal to the identifier.</returns>
  public override bool Equals([NotNullWhen(true)] object? obj) => obj is RoleId id && id.Value == Value;
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
