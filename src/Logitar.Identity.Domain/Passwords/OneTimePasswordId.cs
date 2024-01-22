using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// Represents the identifier of a One-Time Password (OTP).
/// </summary>
public record OneTimePasswordId
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
  /// Initializes a new instance of the <see cref="OneTimePasswordId"/> class.
  /// </summary>
  /// <param name="aggregateId">The aggregate identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public OneTimePasswordId(AggregateId aggregateId, string? propertyName = null)
  {
    new IdValidator(propertyName).ValidateAndThrow(aggregateId.Value);

    AggregateId = aggregateId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordId"/> class.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public OneTimePasswordId(string value, string? propertyName = null)
  {
    value = value.Trim();
    new IdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

  /// <summary>
  /// Creates a new user identifier.
  /// </summary>
  /// <returns>The created identifier.</returns>
  public static OneTimePasswordId NewId() => new(AggregateId.NewId());

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="OneTimePasswordId"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static OneTimePasswordId? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }
}
