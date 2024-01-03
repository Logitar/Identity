using FluentValidation;

namespace Logitar.Identity.Domain;

public static class FluentValidationExtensions
{
  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(value => allowedCharacters == null || value.All(allowedCharacters.Contains))
      .WithErrorCode($"{nameof(AllowedCharacters)}Validator")
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }
}
