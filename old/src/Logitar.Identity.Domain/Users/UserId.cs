using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents the identifier of an user.
/// </summary>
public record UserId
{
  /// <summary>
  /// Gets the aggregate identifier.
  /// </summary>
  public AggregateId AggregateId { get; }
  /// <summary>
  /// Gets the value of the identifier.
  /// </summary>
  public string Value => AggregateId.Value;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserId"/> class.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UserId(Guid id, string? propertyName = null) : this(new AggregateId(id), propertyName)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserId"/> class.
  /// </summary>
  /// <param name="aggregateId">The aggregate identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UserId(AggregateId aggregateId, string? propertyName = null)
  {
    new IdValidator(propertyName).ValidateAndThrow(aggregateId.Value);

    AggregateId = aggregateId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserId"/> class.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UserId(string value, string? propertyName = null)
  {
    value = value.Trim();
    new IdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

  /// <summary>
  /// Creates a new user identifier.
  /// </summary>
  /// <returns>The created identifier.</returns>
  public static UserId NewId() => new(AggregateId.NewId());

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="UserId"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static UserId? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }

  /// <summary>
  /// Converts the identifier to a <see cref="Guid"/>. The conversion will fail if the identifier has not been created from a <see cref="Guid"/>.
  /// </summary>
  /// <returns>The resulting Guid.</returns>
  public Guid ToGuid() => AggregateId.ToGuid();
}
