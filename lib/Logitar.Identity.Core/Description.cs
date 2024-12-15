using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents the textual description of an entity.
/// </summary>
public record Description
{
  /// <summary>
  /// Gets the value of the description.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Description"/> class.
  /// </summary>
  /// <param name="value">The value of the descripion.</param>
  public Description(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="Description"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static Description? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the description.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="Description"/>.
  /// </summary>
  private class Validator : AbstractValidator<Description>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).Description();
    }
  }
}
