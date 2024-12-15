using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents the display name of an entity.
/// </summary>
public record DisplayName
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
  /// Initializes a new instance of the <see cref="DisplayName"/> class.
  /// </summary>
  /// <param name="value">The value of the display name.</param>
  public DisplayName(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="DisplayName"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static DisplayName? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the display name.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="DisplayName"/>.
  /// </summary>
  private class Validator : AbstractValidator<DisplayName>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).DisplayName();
    }
  }
}
