using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents an identifier.
/// </summary>
public record Identifier
{
  /// <summary>
  /// The maximum length of identifiers.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the value of the identifier.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Identifier"/> class.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  public Identifier(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="Identifier"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static Identifier? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the display name.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="Identifier"/>.
  /// </summary>
  private class Validator : AbstractValidator<Identifier>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).Identifier();
    }
  }
}
