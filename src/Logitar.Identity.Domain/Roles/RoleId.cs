using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles;

/// <summary>
/// Represents the identifier of a role.
/// </summary>
public record RoleId
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
  /// Initializes a new instance of the <see cref="RoleId"/> class.
  /// </summary>
  /// <param name="aggregateId">The aggregate identifier.</param>
  public RoleId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleId"/> class.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public RoleId(string value, string? propertyName = null)
  {
    value = value.Trim();
    new AggregateIdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="RoleId"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static RoleId? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }
}
