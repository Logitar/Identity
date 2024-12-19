using FluentValidation;
using FluentValidation.Validators;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to enforce that a date and time are set in the future.
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
public class FutureValidator<T> : IPropertyValidator<T, DateTime>
{
  /// <summary>
  /// Gets the name of the validator.
  /// </summary>
  public string Name { get; } = "FutureValidator";
  /// <summary>
  /// Gets the current moment (now) date and time reference.
  /// </summary>
  public DateTime Now { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="FutureValidator{T}"/> class.
  /// </summary>
  /// <param name="now">The current moment (now) date and time reference.</param>
  public FutureValidator(DateTime? now = null)
  {
    Now = (now ?? DateTime.Now).AsUniversalTime();
  }

  /// <summary>
  /// Returns the default error message template for this validator, when not overridden.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <returns>The default error message template.</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a date and time set in the future.";
  }

  /// <summary>
  /// Validates a specific property value.
  /// </summary>
  /// <param name="context">The validation context.</param>
  /// <param name="value">The value to validate.</param>
  /// <returns>True if the value is valid, or false otherwise.</returns>
  public bool IsValid(ValidationContext<T> context, DateTime value) => value.AsUniversalTime() > Now;
}
