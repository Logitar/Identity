using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class IdentifierValidator : AbstractValidator<string>
{
  public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
  public const int MaximumLength = byte.MaxValue;

  public IdentifierValidator()
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(MaximumLength)
      .AllowedCharacters(AllowedCharacters)
      .Must(x => !char.IsDigit(x.FirstOrDefault()))
        .WithErrorCode(nameof(IdentifierValidator))
        .WithMessage("'{PropertyName}' may not start with a digit.");
  }
}
