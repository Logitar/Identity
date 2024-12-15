using Logitar.EventSourcing;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents the identifier of a tenant.
/// </summary>
public readonly struct TenantId
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
  /// Initializes a new instance of the <see cref="TenantId"/> struct.
  /// </summary>
  /// <param name="value">A Guid value.</param>
  public TenantId(Guid value)
  {
    StreamId = new StreamId(value);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="TenantId"/> struct.
  /// </summary>
  /// <param name="value">A string value.</param>
  public TenantId(string value)
  {
    StreamId = new StreamId(value);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="TenantId"/> struct.
  /// </summary>
  /// <param name="streamId">A stream identifier.</param>
  public TenantId(StreamId streamId)
  {
    StreamId = streamId;
  }

  /// <summary>
  /// Randomly generates a new tenant identifier.
  /// </summary>
  /// <returns>The generated identifier.</returns>
  public static TenantId NewId() => new(StreamId.NewId());

  /// <summary>
  /// Converts the identifier to a <see cref="Guid"/>. The conversion will fail if the identifier has not been created from a <see cref="Guid"/>.
  /// </summary>
  /// <returns>The resulting Guid.</returns>
  public Guid ToGuid() => StreamId.ToGuid();

  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are equal.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are equal.</returns>
  public static bool operator ==(TenantId left, TenantId right) => left.Equals(right);
  /// <summary>
  /// Returns a value indicating whether or not the specified identifiers are different.
  /// </summary>
  /// <param name="left">The first identifier to compare.</param>
  /// <param name="right">The other identifier to compare.</param>
  /// <returns>True if the identifiers are different.</returns>
  public static bool operator !=(TenantId left, TenantId right) => !left.Equals(right);

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the identifier.
  /// </summary>
  /// <param name="obj">The object to be compared to.</param>
  /// <returns>True if the object is equal to the identifier.</returns>
  public override bool Equals([NotNullWhen(true)] object? obj) => obj is TenantId id && id.Value == Value;
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
