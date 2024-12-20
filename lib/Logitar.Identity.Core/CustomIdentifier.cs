using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents a custom identifier of an entity.
/// </summary>
public record CustomIdentifier
{
  /// <summary>
  /// The maximum length of custom identifiers.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the value of the custom identifier.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifier"/> class.
  /// </summary>
  /// <param name="value">The value of the custom identifier.</param>
  public CustomIdentifier(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="CustomIdentifier"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static CustomIdentifier? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the custom identifier.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="CustomIdentifier"/>.
  /// </summary>
  private class Validator : AbstractValidator<CustomIdentifier>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).CustomIdentifier();
    }
  }
}
