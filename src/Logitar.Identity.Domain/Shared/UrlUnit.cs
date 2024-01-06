using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

public record UrlUnit
{
  public const int MaximumLength = 2048;
  public const string SafeCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

  public Uri Uri { get; }
  public string Value => Uri.ToString();

  public UrlUnit(string value)
  {
    value = value.Trim();
    new UrlValidator().ValidateAndThrow(value);

    Uri = new(value);
  }

  public static UrlUnit? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);
}
