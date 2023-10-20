using FluentValidation;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The validator used to validate person names.
/// See <see cref="PersonNameUnit"/> for more information.
/// </summary>
public class PersonNameValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PersonNameValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public PersonNameValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(PersonNameUnit.MaximumLength)
      .WithPropertyName(propertyName);
  }
}
