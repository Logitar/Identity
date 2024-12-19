using FluentValidation;
using FluentValidation.Validators;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// TODO(fpion): document
/// </summary>
/// <typeparam name="T">TODO(fpion): document</typeparam>
public class PastValidator<T> : IPropertyValidator<T, DateTime>
{
  /// <summary>
  /// TODO(fpion): document
  /// </summary>
  public string Name { get; } = "PastValidator";
  /// <summary>
  /// TODO(fpion): document
  /// </summary>
  public DateTime Now { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="PastValidator{T}"/> class.
  /// </summary>
  /// <param name="now">TODO(fpion): document</param>
  public PastValidator(DateTime? now = null)
  {
    Now = (now ?? DateTime.Now).AsUniversalTime();
  }

  /// <summary>
  /// TODO(fpion): document
  /// </summary>
  /// <param name="errorCode">TODO(fpion): document</param>
  /// <returns>TODO(fpion): document</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a date and time set in the past.";
  }

  /// <summary>
  /// TODO(fpion): document
  /// </summary>
  /// <param name="context">TODO(fpion): document</param>
  /// <param name="value">TODO(fpion): document</param>
  /// <returns>TODO(fpion): document</returns>
  public bool IsValid(ValidationContext<T> context, DateTime value) => value.AsUniversalTime() < Now;
}
