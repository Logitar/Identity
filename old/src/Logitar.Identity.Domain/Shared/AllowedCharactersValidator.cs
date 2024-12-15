using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to enforce a strict list of characters.
/// </summary>
public class AllowedCharactersValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AllowedCharactersValidator"/> class.
  /// </summary>
  /// <param name="allowedCharacters">The allowed characters.</param>
  public AllowedCharactersValidator(string? allowedCharacters)
  {
    RuleFor(x => x).Must(s => allowedCharacters == null || s.All(allowedCharacters.Contains))
      .WithErrorCode(nameof(AllowedCharactersValidator))
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }
}
