using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate type identifiers.
/// See <see cref="TimeZoneUnit"/> for more information.
/// </summary>
public class IdentifierValidator : AbstractValidator<string>
{
  /// <summary>
  /// The allowed characters in type identifiers.
  /// </summary>
  public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
  /// <summary>
  /// The maximum length of type identifiers.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Initializes a new instance of the <see cref="IdentifierValidator"/> class.
  /// </summary>
  public IdentifierValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(MaximumLength)
      .SetValidator(new AllowedCharactersValidator(AllowedCharacters))
      .Must(x => !char.IsDigit(x.FirstOrDefault()))
        .WithErrorCode(nameof(IdentifierValidator))
        .WithMessage("'{PropertyName}' may not start with a digit.");
  }
}
