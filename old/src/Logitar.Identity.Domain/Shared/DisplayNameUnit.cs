using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents the display name of an entity.
/// </summary>
public record DisplayNameUnit
{
  /// <summary>
  /// The maximum length of display names.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the value of the display name.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DisplayNameUnit"/> class.
  /// </summary>
  /// <param name="value">The value of the display name.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public DisplayNameUnit(string value, string? propertyName = null)
  {
    Value = value.Trim();
    new DisplayNameValidator(propertyName).ValidateAndThrow(Value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="DisplayNameUnit"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the display name.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static DisplayNameUnit? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value.Trim(), propertyName);
  }
}
