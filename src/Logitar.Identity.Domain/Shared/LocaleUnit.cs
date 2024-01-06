using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public class LocaleUnit
{
  public const int MaximumLength = 16;


  public CultureInfo Culture { get; }
  public string Code => Culture.Name;

  public LocaleUnit(string code)
  {
    code = code.Trim();
    new LocaleValidator().ValidateAndThrow(code);

    Culture = CultureInfo.GetCultureInfo(code.Trim());
  }

  public static LocaleUnit? TryCreate(string? code) => string.IsNullOrWhiteSpace(code) ? null : new(code);
}
