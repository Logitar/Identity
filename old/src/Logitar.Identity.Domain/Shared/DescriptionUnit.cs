using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents the textual description of an entity.
/// </summary>
public record DescriptionUnit
{
  /// <summary>
  /// Gets the value of the description.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DescriptionUnit"/> class.
  /// </summary>
  /// <param name="value">The value of the description.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public DescriptionUnit(string value, string? propertyName = null)
  {
    Value = value.Trim();
    new DescriptionValidator(propertyName).ValidateAndThrow(Value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="DescriptionUnit"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the description.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static DescriptionUnit? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }
}
