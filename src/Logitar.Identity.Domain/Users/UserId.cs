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
  /// <param name="aggregateId">The aggregate identifier.</param>
  public UserId(AggregateId aggregateId)
  {
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
    new AggregateIdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

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
}
