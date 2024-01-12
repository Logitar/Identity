using FluentValidation;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate aggregate identifiers.
/// See <see cref="AggregateId"/> for more information.
/// </summary>
public class IdValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AggregateId"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public IdValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(AggregateId.MaximumLength)
      .SetValidator(new AllowedCharactersValidator(UrlUnit.SafeCharacters))
      .WithPropertyName(propertyName);
  }
}
