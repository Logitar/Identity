using FluentValidation;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// Represents the validator for instances of <see cref="IEmail"/>.
/// </summary>
public class EmailValidator : AbstractValidator<IEmail>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EmailValidator"/> class.
  /// </summary>
  public EmailValidator()
  {
    RuleFor(x => x.Address).NotEmpty().MaximumLength(Email.MaximumLength).EmailAddress();
  }
}
