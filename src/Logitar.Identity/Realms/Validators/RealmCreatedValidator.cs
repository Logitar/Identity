using FluentValidation;
using Logitar.Identity.Realms.Events;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="RealmCreatedEvent"/> class.
/// </summary>
internal class RealmCreatedValidator : RealmSavedValidator<RealmCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmCreatedValidator"/> class.
  /// </summary>
  public RealmCreatedValidator()
  {
    RuleFor(x => x.UniqueName).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Alias();
  }
}
