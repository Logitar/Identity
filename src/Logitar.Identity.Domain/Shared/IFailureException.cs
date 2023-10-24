using FluentValidation.Results;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Describes exceptions that can be represented by a validation failure.
/// See <see cref="Exception"/> and <see cref="ValidationFailure"/> for more information.
/// </summary>
public interface IFailureException
{
  /// <summary>
  /// Gets the validation failure of the exception.
  /// </summary>
  ValidationFailure Failure { get; }
}
