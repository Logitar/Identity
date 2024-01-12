using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

/// <summary>
/// The validator used to validate email addresses.
/// See <see cref="EmailUnit"/> for more information.
/// </summary>
public class EmailValidator : AbstractValidator<IEmail>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EmailValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public EmailValidator(string? propertyName = null)
  {
    RuleFor(x => x.Address).NotEmpty()
      .MaximumLength(EmailUnit.MaximumLength)
      .EmailAddress()
      .WithPropertyName(propertyName == null ? null : $"{propertyName}.{nameof(IEmail.Address)}");
  }
}
